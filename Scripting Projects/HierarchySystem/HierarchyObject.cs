using System.Collections.Generic;
using System.Linq;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// Represents any item that is part of the hierarchy
	/// </summary>
	public abstract class HierarchyObject
	{
		public HierarchyObject(HierarchyObject root = null, HierarchyObject parent = null)
		{
			hierarchyRoot = root;
			this.parent = parent;
		}

		/// <summary>
		/// Is this HierarchyObject at the root of the hierarchy?
		/// </summary>
		public bool IsRoot
		{
			get
			{
				if (HierarchySystem.LoadedHierarchies.ContainsValue(this)) // Extra check that this simply wasnt incorrectly initialized
				{
					return (parent == null); // If this HierarchyObject has no parent that means that it has to be at the root, since HierarchySystem cannot be used as parent
				}
				else
					return false;
			}
		}

		/// <summary>
		/// The private field referencing this Hierarchy´s root.
		/// </summary>
		private HierarchyObject hierarchyRoot;
		/// <summary>
		/// Returns the root of this Hierarchy.
		/// </summary>
		public HierarchyObject HierarchyRoot
		{
			get => hierarchyRoot;
		}

		/// <summary>
		/// Returns the name of the containing hierarchy, or calls the HierarchySystem´s rename method if set.
		/// </summary>
		public string HierarchyName
		{
			get => HierarchySystem.GetName(HierarchyRoot);
			set
			{
				HierarchySystem.SetName(HierarchyRoot, value);
			}
		}

		/// <summary>
		/// The field referencing this HierarchyObject´s parent in the Hierarchy.
		/// </summary>
		private HierarchyObject parent;
		/// <summary>
		/// Returns the parent, but does not allow setting it.
		/// </summary>
		public object Parent
		{
			get => parent;
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
					return parent.GetName(this);
				}
				else
				{
					return HierarchySystem.GetName(this);
				}
			}
			set => parent.SetName(this, value);
		}

		/// <summary>
		/// This method sets the name of this 
		/// </summary>
		/// <param name="hierarchyObject">The HierarchyObject that should recieve a name change</param>
		/// <param name="newName">The new name for the object</param>
		private void SetName(HierarchyObject hierarchyObject, string newName)
		{
			string key = localHierarchy.FirstOrDefault(x => x.Value == hierarchyObject).Key;
			localHierarchy.Remove(key);
			localHierarchy.Add(newName, hierarchyObject);
		}

		/// <summary>
		/// Gets the name of a child HierarchyObject.
		/// </summary>
		/// <param name="hierarchyObject">The HierarchyObject to get the name of</param>
		/// <returns>The name of this object</returns>
		private string GetName(HierarchyObject hierarchyObject)
		{
			string key;
			key = localHierarchy.FirstOrDefault(x => x.Value == hierarchyObject).Key; // Get the key of this hierarchyRoot. Lets hope you dont have duplicate hierarchyRoots though... hmmm lets TODO that
			return key;
		}

		/// <summary>
		/// The entire current hierarchy from the root, for users modifying pleasure.
		/// </summary>
		public Dictionary<string, HierarchyObject> Hierarchy
		{
			get => HierarchyRoot.LocalHierarchy;
		}

		/// <summary>
		/// The local hierarchy, containing all child HierarchyObjects that this HierarchyObject has.
		/// </summary>
		protected Dictionary<string, HierarchyObject> localHierarchy = new Dictionary<string, HierarchyObject>();
		/// <summary>
		/// The public LocalHierarchy. It containins all child HierarchyObjects that this HierarchyObject has.
		/// </summary>
		public Dictionary<string, HierarchyObject> LocalHierarchy => localHierarchy;

		/// <summary>
		/// Adds the HierarchyObject and changes it´s parent.
		/// </summary>
		/// <param name="name">The name to use in the reparent</param>
		/// <param name="hierarchyObject">The HierarchyObject to reparent</param>
		public void ReParent(string name, HierarchyObject hierarchyObject)
		{
			LocalHierarchy.Add(name, hierarchyObject);
			hierarchyObject.parent = this;
		}

		/// <summary>
		/// Follows a path, relatively from this point.
		/// </summary>
		/// <param name="path">The path to follow. HierarchyObjects are separated by '/'</param>
		/// <returns>The HierarchyObject at the end of the path</returns>
		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/'); // Split the path into the individual HierarchyObjects to follow 

			if (pathSegments.Length == 1) // We have reached the end of the path, this HierarchyObject is the destination!
			{
				return this; // Return this HierarchyObject
			}

			string pathToFollow = path.Remove(0, pathSegments[0].Length + 1); // Remove the top level HierarchyObject from the path as we continue forwards
			string nextObject = pathSegments[1]; // The next HierarchyObject´s name

			return localHierarchy[nextObject].FollowPath(pathToFollow); // We are going to do some of that sweet bitter sweet recursion magic by returning the result of a follow call to the HierarchyObject that is next in the path.
		}
	}
}
