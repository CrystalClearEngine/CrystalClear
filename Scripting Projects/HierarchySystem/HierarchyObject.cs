using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// Represents any item that is part of the hierarchy
	/// </summary>
	public abstract class HierarchyObject // TODO make FindHierarchyObjects method like in Script
	{
		/// <summary>
		/// OnCreate is called when the HierarchyObject is created in a Hierarchy for the first time.
		/// </summary>
		protected virtual void OnCreate()
		{
			// Do some initialization maybe? It's all up to you...
		}

		/// <summary>
		/// OnLocalHierarchyChange is called when the LocalHierarchy is modified.
		/// </summary>
		protected virtual void OnLocalHierarchyChange()
		{

		}

		/// <summary>
		/// OnReparent is called when the HierarchyObject's parent updates.
		/// </summary>
		/// <param name="newParent">The new parent.</param>
		protected virtual void OnReparent(HierarchyObject newParent)
		{

		}

		/// <summary>
		/// Adds a Script to the HierarchyObject.
		/// </summary>
		/// <param name="hierarchyObject">The HierarchyObject to add the Script to.</param>
		/// <param name="script">The Script to add to the HierarchyObject.</param>
		/// <returns>The resulting HierarchyObject.</returns>
		public static HierarchyObject operator + (HierarchyObject hierarchyObject, Script script)
		{
			HierarchyObject result = hierarchyObject;
			result.AddScript(script);
			return result;
		}

		/// <summary>
		/// The scripts that are currently attatched to this object.
		/// </summary>
		public List<Script> Scripts = new List<Script>(); // TODO use directory, allow naming of attatched scripts. Also maybe rename to componnents, or maybe that should be it's own separate thing (they can be like data containers etc, or maybe don't need to exist at all or under a different name).

		/// <summary>
		/// Adds a script based on the specified type to this HierarchyObject.
		/// </summary>
		/// <param name="scriptType">The type of the script we are going to add.</param>
		public void AddScript(Type scriptType) => AddScript(new Script(this, scriptType));
		/// <summary>
		/// Adds a script to this HierarchyObject.
		/// </summary>
		/// <param name="script">The script to add.</param>
		public void AddScript(Script script)
		{
			Scripts.Add(script);
		}

		/// <summary>
		/// Is this HierarchyObject the root of the entire hierarchy (has no parent)?
		/// </summary>
		public bool IsRoot
		{
			get
			{
				// If this HierarchyObject has no parent that means that it has to be at the root, since HierarchySystem cannot be used as parent.
				return (Parent == null);
			}
		}

		/// <summary>
		/// Checks wether or not this Hierarchy is in the LoadedHierarchies' list.
		/// </summary>
		public bool IsLoadedHierarchy
		{
			get
			{
				return HierarchySystem.LoadedHierarchies.Values.Contains(this);
			}
		}

		/// <summary>
		/// Returns the root of this Hierarchy.
		/// </summary>
		public HierarchyObject Root
		{
			get
			{
				// Is this the root?
				if (IsRoot)
				{
					return this;
				}
				// If not, then we should refer to our parent. They should know :).
				else
				{
					return Parent.Root;
				}
			}
		}

		/// <summary>
		/// Returns the name of the containing hierarchy, or calls the HierarchySystem's rename method if set.
		/// </summary>
		public string HierarchyName
		{
			get => HierarchySystem.GetHierarchyName(Root);
			set
			{
				HierarchySystem.SetHierarchyName(Root, value);
			}
		}

		/// <summary>
		/// The field referencing this HierarchyObject's parent in the Hierarchy.
		/// </summary>
		private HierarchyObject parent;
		/// <summary>
		/// Returns the parent, and utlizes ReParentChild() to set it.
		/// </summary>
		public HierarchyObject Parent
		{
			get => parent;
			set
			{
				ReParentThis(value);
				OnReparent(value);
			}
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
					return Parent.GetChildName(this);
				}
				else
				{
					return HierarchySystem.GetHierarchyName(this);
				}
			}
			set => Parent.SetChildName(this, value);
		}

		/// <summary>
		/// The entire current hierarchy from the root, for scripts modifying pleasure.
		/// </summary>
		protected ImmutableDictionary<string, HierarchyObject> Hierarchy => Root.LocalHierarchy;

		/// <summary>
		/// The local hierarchy, containing all child HierarchyObjects that this HierarchyObject has.
		/// </summary>
		private ImmutableDictionary<string, HierarchyObject> localHierarchy = ImmutableDictionary<string, HierarchyObject>.Empty;
		/// <summary>
		/// The publicly accessible LocalHierarchy.
		/// </summary>
		public ImmutableDictionary<string, HierarchyObject> LocalHierarchy
		{
			get => localHierarchy;
			set
			{
				OnLocalHierarchyChange();
				localHierarchy = value;
			}
		}

		#region HierarchyManagement
		/// <summary>
		/// This method sets the name of the specified child to the specified new key. 
		/// </summary>
		/// <param name="child">The HierarchyObject that should recieve a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(HierarchyObject child, string newName)
		{
			string key = GetChildName(child);
			RemoveChild(key);
			AddChild(newName, child);
		}

		/// <summary>
		/// This method sets the name of the specified key in the LocalHierarchy to the new key. 
		/// </summary>
		/// <param name="currentName">The HierarchyObject that should recieve a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(string currentName, string newName)
		{
			HierarchyObject hierarchyObject = LocalHierarchy[currentName];
			RemoveChild(currentName);
			AddChild(newName, hierarchyObject);
		}

		/// <summary>
		/// Returns the name of a child HierarchyObject that is present in the LocalHierarchy of this HierarchyObject.
		/// </summary>
		/// <param name="child">The HierarchyObject to get the name of.</param>
		/// <returns>The name of this object.</returns>
		public string GetChildName(HierarchyObject child)
		{
			string key = LocalHierarchy.First(x => ReferenceEquals(x.Value, child)).Key;
			return key;
		}

		/// <summary>
		/// Adds a HierarchyObject to the LocalHierarchy and also set the HierarchyObject's parent accordingly
		/// </summary>
		/// <param name="name">The name of the HierarchyObject to add.</param>
		/// <param name="child">The HierarchyObject to add.</param>
		public void AddChild(string name, HierarchyObject child)
		{
			LocalHierarchy.Add(name, child);
			child.SetUp(this);
			OnLocalHierarchyChange();
		}

		/// <summary>
		/// Changes the parent of a HierarchyObject in the LocalHierarchy.
		/// </summary>
		/// <param name="newParent">The parent to move the child to.</param>
		/// <param name="child">The child to move.</param>
		public void ReParentChild(HierarchyObject newParent, HierarchyObject child)
		{
			string childName = child.Name;
			RemoveChild(child);
			newParent.AddChild(childName, child);
		}

		/// <summary>
		/// Changes the parent of a HierarchyObject in a Hierarchy.
		/// </summary>
		/// <param name="oldParent">The parent to remove the HierarchyObject from.</param>
		/// <param name="newParent">The parent to add the HierarchyObject to.</param>
		/// <param name="child">The child object to re-parent.</param>
		public static void ReParent(HierarchyObject oldParent, HierarchyObject newParent, HierarchyObject child)
		{
			string childName = child.Name;
			oldParent.RemoveChild(child);
			newParent.AddChild(childName, child);
		}

		/// <summary>
		/// Changes the parent of this HierarchyObject.
		/// </summary>
		/// <param name="newParent">The parent to add the HierarchyObject to.</param>
		public void ReParentThis(HierarchyObject newParent)
		{
			string childName = Name;
			if (!IsRoot)
			{
				Parent.RemoveChild(childName);
			}
			newParent.AddChild(childName, this);
		}

		/// <summary>
		/// Sets up the HierarchyObject.
		/// </summary>
		/// <param name="parent">Optional parent override.</param>
		public void SetUp(HierarchyObject parent = null)
		{
			if (Parent == null && parent == null) // Parent null check.
			{
				throw new Exception("No parent specified! Please set the parent before calling or include it as a parameter.");
			}

			if (parent != null) // The parent parameter isn't at default value, need to set the current object parent.
			{
				this.parent = parent;
			}

			OnCreate();
		}

		/// <summary>
		/// Removes the specified child by HierarchyObject from the LocalHierarchy.
		/// </summary>
		/// <param name="child">The child HierarchyObject to remove.</param>
		public void RemoveChild(HierarchyObject child)
		{
			KeyValuePair<string, HierarchyObject> item = LocalHierarchy.First(HierarchyObject => HierarchyObject.Value == child);

			RemoveChild(item.Key);
		}

		/// <summary>
		/// Removes the specified child by name from the LocalHierarchy.
		/// </summary>
		/// <param name="childName">The child's name.</param>
		public void RemoveChild(string childName) => LocalHierarchy.Remove(childName);

		/// <summary>
		/// Follows a path relatively from this point and returns the specified HierarchyObject.
		/// </summary>
		/// <param name="path">The path to follow. HierarchyObjects are separated by '/'.</param>
		/// <returns>The HierarchyObject at the end of the path.</returns>
		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/'); // Split the path into the individual HierarchyObjects to follow.

			if (pathSegments.Length == 1) // We have reached the end of the path, this HierarchyObject is the destination!
			{
				return this; // Return this HierarchyObject.
			}

			string pathToFollow = path.Remove(0, pathSegments[0].Length + 1); // Remove the top level HierarchyObject from the path as we continue forwards.
			string nextObject = pathSegments[1]; // The next HierarchyObject's name.

			return LocalHierarchy[nextObject].FollowPath(pathToFollow); // We are going to do some of that sweet bitter sweet recursion magic by returning the result of a follow call to the HierarchyObject that is next in the path.
		}
		#endregion
	}
}
