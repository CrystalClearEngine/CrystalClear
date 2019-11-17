using System;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// Represents any item that is part of the hierarchy
	/// </summary>
	public abstract class HierarchyObject
	{
		protected HierarchyObject() // This is for Scripts only! Using this for HierarchyObjects might cause problems for users who want to script it...
		{
		}

		public HierarchyObject(HierarchyRoot hierarchyRoot = null, HierarchyObject parent = null) // ...And this is only for derived HierarchyObjectTypes!
		{
			this.hierarchyRoot = hierarchyRoot;
			this.parent = parent;
		}

		public bool IsAtRoot
		{
			get
			{
				return parent == null; // We know that we are at the root if our parent is null (parent has to be a HierarchyRoot)
			}
		}

		private HierarchyRoot hierarchyRoot;
		public HierarchyRoot HierarchyRoot
		{
			get => hierarchyRoot;
		}

		public string HierarchyName
		{
			get => hierarchyRoot.Name;
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
			get => hierarchyRoot.Name;
		}

		public Dictionary<string, HierarchyObject> Hierarchy
		{
			get
			{
				return HierarchyRoot.HierarchyObjects;
			}
		}

		public Dictionary<string, HierarchyObject> LocalHierarchy = new Dictionary<string, HierarchyObject>();

		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			if (pathSegments.Length == 0) // We have reached the end of the path, this is the destination!
			{
				return this;
			}
			return LocalHierarchy[pathSegments[0]].FollowPath(path.Remove(0, pathSegments[0].Length + 1));
		}
	}
}
