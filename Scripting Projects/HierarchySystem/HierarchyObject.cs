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
		public HierarchyObject(HierarchyObject hierarchyRoot = null, HierarchyObject parent = null) // ...And this is only for derived HierarchyObjectTypes!
		{
			this.root = hierarchyRoot;
			this.parent = parent;
		}

		public bool IsAtRoot
		{
			get
			{
				return (parent == null); // We know that we are the root if our parent is null (parent has to be a HierarchyRoot)
			}
		}

		private HierarchyObject root;
		public HierarchyObject HierarchyRoot
		{
			get => root;
		}

		public string HierarchyName
		{
			get => root.Name;
			set
			{
				HierarchySystem.SetName(HierarchyRoot, value);
			}
		}

		private HierarchyObject parent;
		public object Parent
		{
			get
			{
				if (IsAtRoot == false)
					return parent;
				else
					return HierarchyRoot;
			}
		}

		public string Name
		{
			get
			{
				if (IsAtRoot == false)
				{
					return parent.GetName(this);
				}
				else
					return HierarchySystem.GetName(this);
			}
			set => parent.SetName(this, value);
		}

		private void SetName(HierarchyObject hierarchyObject, string newName)
		{
			string key = localHierarchy.FirstOrDefault(x => x.Value == hierarchyObject).Key;
			localHierarchy.Remove(key);
			localHierarchy.Add(newName, hierarchyObject);
		}

		private string GetName(HierarchyObject hierarchyObject)
		{
			string key;
			key = localHierarchy.FirstOrDefault(x => x.Value == hierarchyObject).Key; // Get the key of this hierarchyRoot. Lets hope you dont have duplicate hierarchyRoots though... hmmm lets TODO that
			return key;
		}

		public Dictionary<string, HierarchyObject> Hierarchy
		{
			get => HierarchyRoot.LocalHierarchy;
		}

		private Dictionary<string, HierarchyObject> localHierarchy = new Dictionary<string, HierarchyObject>();
		public Dictionary<string, HierarchyObject> LocalHierarchy => localHierarchy;

		public void Add(string path, HierarchyObject hierarchyObject)
		{

		}

		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			string pathToFollow = path.Remove(0, pathSegments[0].Length + 1);
			string nextObject = pathSegments[1];

			if (pathSegments.Length <= 1) // We have reached the end of the path, this is the destination!
			{
				return this;
			}

			return localHierarchy[nextObject].FollowPath(pathToFollow);
		}
	}
}
