using System;
using System.Collections.Generic;
using System.Linq;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// Represents any item that is part of the hierarchy
	/// </summary>
	public abstract class HierarchyObject
	{
		/// <summary>
		/// Is this HierarchyObject the root of the entiere hierarchy (has no parent, is part of the LoadedHierarchies)?
		/// </summary>
		public bool IsRoot
		{
			get
			{
				if (HierarchySystem.LoadedHierarchies.ContainsValue(this)) // Extra check that this simply wasnt incorrectly initialized.
				{
					return (parent == null); // If this HierarchyObject has no parent that means that it has to be at the root, since HierarchySystem cannot be used as parent.
				}
				else
					return false;
			}
		}

		/// <summary>
		/// The private field referencing this Hierarchy´s root.
		/// </summary>
		private HierarchyObject root;
		/// <summary>
		/// Returns the root of this Hierarchy.
		/// </summary>
		public HierarchyObject Root
		{
			get => root;
		}

		/// <summary>
		/// Returns the name of the containing hierarchy, or calls the HierarchySystem´s rename method if set.
		/// </summary>
		public string HierarchyName
		{
			get => HierarchySystem.GetName(Root);
			set
			{
				HierarchySystem.SetName(Root, value);
			}
		}

		/// <summary>
		/// The field referencing this HierarchyObject´s parent in the Hierarchy.
		/// </summary>
		private HierarchyObject parent;
		/// <summary>
		/// Returns the parent, and utlizes ReParentChild() to set it.
		/// </summary>
		public HierarchyObject Parent
		{
			get => parent;
			set => ReParentThis(value);
		}

		/// <summary>
		/// Returns the name of this HierarchyObject by looking it up via GetName() on the parent (or HierarchySystem if this HierarchyObject is the root). This property supports setting the name, which uses the SetName() method on the parent or HierarchySystem if this Hierarchy object is the root.
		/// </summary>
		public string Name
		{
			get
			{
				if (IsRoot == false)
				{
					return parent.GetChildName(this);
				}
				else
				{
					return HierarchySystem.GetName(this);
				}
			}
			set => parent.SetChildName(this, value);
		}

		/// <summary>
		/// The entire current hierarchy from the root, for scripts modifying pleasure.
		/// </summary>
		protected Dictionary<string, HierarchyObject> Hierarchy => localHierarchy;

		/// <summary>
		/// The local hierarchy, containing all child HierarchyObjects that this HierarchyObject has.
		/// </summary>
		protected Dictionary<string, HierarchyObject> localHierarchy = new Dictionary<string, HierarchyObject>();
		/// <summary>
		/// The public LocalHierarchy. It containins all child HierarchyObjects that this HierarchyObject has.
		/// </summary>
		public Dictionary<string, HierarchyObject> LocalHierarchy => localHierarchy;

		#region HierarchyManagement
		/// <summary>
		/// This method sets the name of the specified child to the specified new key. 
		/// </summary>
		/// <param name="child">The HierarchyObject that should recieve a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(HierarchyObject child, string newName)
		{
			string key = localHierarchy.FirstOrDefault(x => x.Value == child).Key;
			localHierarchy.Remove(key);
			localHierarchy.Add(newName, child);
		}

		/// <summary>
		/// This method sets the name of the specified key in the LocalHierarchy to the new key. 
		/// </summary>
		/// <param name="childName">The HierarchyObject that should recieve a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(string childName, string newName)
		{
			HierarchyObject hierarchyObject = localHierarchy[childName];
			localHierarchy.Remove(childName);
			localHierarchy.Add(newName, hierarchyObject);
		}

		/// <summary>
		/// Returns the name of a child HierarchyObject that is present in the LocalHierarchy of this HierarchyObject.
		/// </summary>
		/// <param name="child">The HierarchyObject to get the name of</param>
		/// <returns>The name of this object</returns>
		public string GetChildName(HierarchyObject child)
		{
			string key;
			key = LocalHierarchy.FirstOrDefault(x => x.Value == child).Key; // Get the key of this hierarchyRoot. Lets hope you dont have duplicate hierarchyRoots though... hmmm lets TODO that
			if (!ReferenceEquals(LocalHierarchy[key], child))
				throw new Exception("Cannot find object, this system is currently broken tho, will fix l8tr");
			return key;
		}

		/// <summary>
		/// Adds a HierarchyObject to the LocalHierarchy and also set the HierarchyObject´s parent accordingly
		/// </summary>
		/// <param name="name">The name of the HierarchyObject to add</param>
		/// <param name="child">The HierarchyObject to add</param>
		public void AddChild(string name, HierarchyObject child)
		{
			LocalHierarchy.Add(name, child);
			child.SetUp(this);
		}
		
		/// <summary>
		/// Changes the parent of a HierarchyObject in the direct LocalHierarchy.s
		/// </summary>
		/// <param name="newParent">The parent to move the child to</param>
		/// <param name="child">The child to move</param>
		public void ReParentChild(HierarchyObject newParent, HierarchyObject child)
		{
			string childName = child.Name;
			RemoveChild(child);
			newParent.AddChild(childName, child);
		}

		/// <summary>
		/// Changes the parent of a HierarchyObject in a Hierarchy.
		/// </summary>
		/// <param name="oldParent">The parent to remove the HierarchyObject from</param>
		/// <param name="newParent">The parent to add the HierarchyObject to</param>
		/// <param name="child">The child object to re-parent</param>
		public static void ReParent(HierarchyObject oldParent, HierarchyObject newParent, HierarchyObject child)
		{
			string childName = child.Name;
			oldParent.RemoveChild(child);
			newParent.AddChild(childName, child);
		}

		/// <summary>
		/// Changes the parent of this HierarchyObject.
		/// </summary>
		/// <param name="oldParent">The parent to remove the HierarchyObject from</param>
		/// <param name="newParent">The parent to add the HierarchyObject to</param>
		/// <param name="child">The child object to re-parent</param>
		public void ReParentThis(HierarchyObject newParent)
		{
			string childName = Name;
			parent.RemoveChild(childName);
			newParent.AddChild(childName, this);
		}

		/// <summary>
		/// Sets up the HierarchyObject.
		/// </summary>
		/// <param name="parent">Optional parent override</param>
		public void SetUp(HierarchyObject parent = null)
		{
			if (this.parent == null && parent == null) // Parent null check.
				throw new Exception("No parent specified! Please set the parent before calling or include it as a parameter.");

			if (parent != null) // The parent parameter isn´t at default value, need to set the current object parent.
			{
				this.parent = parent;
			}

			root = parent.root; // Set the HierarchyRoot of this HierarchyObject.
		}

		/// <summary>
		/// Removes the specified child by HierarchyObject from the LocalHierarchy.
		/// </summary>
		/// <param name="child">The child HierarchyObject to remove</param>
		public void RemoveChild(HierarchyObject child)
		{
			KeyValuePair<string, HierarchyObject> item = localHierarchy.First(HierarchyObject => HierarchyObject.Value == child);

			localHierarchy.Remove(item.Key);
		}

		/// <summary>
		/// Removes the specified child by name from the LocalHierarchy.
		/// </summary>
		/// <param name="child">The child´s name</param>
		public void RemoveChild(string childName)
		{
			localHierarchy.Remove(childName);
		}

		/// <summary>
		/// Follows a path relatively from this point and returns the specified HierarchyObject.
		/// </summary>
		/// <param name="path">The path to follow. HierarchyObjects are separated by '/'</param>
		/// <returns>The HierarchyObject at the end of the path</returns>
		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/'); // Split the path into the individual HierarchyObjects to follow.

			if (pathSegments.Length == 1) // We have reached the end of the path, this HierarchyObject is the destination!
			{
				return this; // Return this HierarchyObject.
			}

			string pathToFollow = path.Remove(0, pathSegments[0].Length + 1); // Remove the top level HierarchyObject from the path as we continue forwards.
			string nextObject = pathSegments[1]; // The next HierarchyObject´s name.

			return localHierarchy[nextObject].FollowPath(pathToFollow); // We are going to do some of that sweet bitter sweet recursion magic by returning the result of a follow call to the HierarchyObject that is next in the path.
		}
		#endregion
	}
}
