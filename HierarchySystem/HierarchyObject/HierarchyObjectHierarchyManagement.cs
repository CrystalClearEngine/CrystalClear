﻿using System;
using System.Linq;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObject
	{
		/// <summary>
		/// The local hierarchy, containing all child HierarchyObjects that this HierarchyObject has.
		/// </summary>
		private Hierarchy localHierarchy = null;
		public Hierarchy LocalHierarchy
		{
			get
			{
				if (localHierarchy is null)
				{
					localHierarchy = new Hierarchy(this);
				}
				return localHierarchy;
			}
		}

		/// <summary>
		/// Accesses the LocalHierarchy of this HierarchyObject.
		/// </summary>
		public HierarchyObject this[string index]
		{
			get
			{
				return LocalHierarchy[index];
			}
			set
			{
				LocalHierarchy[index] = value;
			}
		}

		/// <summary>
		/// The field referencing this HierarchyObject's parent in the Hierarchy.
		/// </summary>
		private WeakReference<HierarchyObject> parent = new WeakReference<HierarchyObject>(null);
		/// <summary>
		/// Returns the parent, or utlizes ReParentChild() to set it.
		/// </summary>
		public HierarchyObject Parent
		{
			get
			{
				parent.TryGetTarget(out HierarchyObject hierarchyObject);/* ?? throw new Exception("This HierarchyObject has no parent! Please check using IsRoot beforehand.")*/;
				return hierarchyObject;
			}

			set
			{
				ReParentThis(value);
			}
		}

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
			LocalHierarchy.AddChild(name, child);
			child.SetUp(false, this);
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
		// TODO: make IsRoot into a field?
		public void SetUp(bool IsRoot = false, HierarchyObject parent = null)
		{
			if (Parent is null && parent is null && !IsRoot)
			{
				throw new ArgumentException("No parent specified for non root HierarchyObject! Please set the parent before calling or include it as a parameter.");
			}

			if (parent != null) // The parent parameter isn't at default value, need to set the current object parent.
			{
				this.parent.SetTarget(parent);
			}

			EventSystem.EventSystem.UnsubscribeEvents(GetType(), this);

			// TODO: do this in a constructor instead so it is only done once?

			// Subscribe all events this HierarchyObject has.
			EventSystem.EventSystem.SubscribeEvents(GetType(), this);

			OnSetUp();
			OnReparent(Parent);
		}

		/// <summary>
		/// Removes the specified child specified by HierarchyObject from the LocalHierarchy.
		/// </summary>
		/// <param name="child">The child HierarchyObject to remove.</param>
		public void RemoveChild(HierarchyObject child)
		{
			string key = LocalHierarchy.First(HierarchyObject => HierarchyObject.Value == child).Key;

			RemoveChild(key);
		}

		/// <summary>
		/// Removes the specified child specified by name from the LocalHierarchy.
		/// </summary>
		/// <param name="childName">The child's name.</param>
		public void RemoveChild(string childName)
		{
			EventSystem.EventSystem.UnsubscribeEvents(LocalHierarchy[childName].GetType(), LocalHierarchy[childName].GetType());

			LocalHierarchy[childName].RemoveAllScripts();

			LocalHierarchy.RemoveChild(childName);
		}

		/// <summary>
		/// Follows a path relatively from this point and returns the specified HierarchyObject.
		/// </summary>
		/// <param name="path">The path to follow. Hierarchy layers are separated by '/'.</param>
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
	}
}