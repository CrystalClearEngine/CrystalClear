using System;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// Represents any item that is part of the hierarchy
	/// </summary>
	public abstract class HierarchyObject
	{
		public void SetUp(HierarchyRoot hierarchyRoot)
		{
			this.hierarchyRoot = hierarchyRoot;
		}

		private HierarchyRoot hierarchyRoot;
		public HierarchyRoot HierarchyRoot
		{
			get => hierarchyRoot;
			private set { }
		}

		public Dictionary<string, HierarchyObject> Hierarchy
		{
			get
			{
				return HierarchyRoot.HierarchyObjects;
			}
			private set
			{
				throw new NotSupportedException();
			}
		}


		public Dictionary<string, HierarchyObject> LocalHierarchy = new Dictionary<string, HierarchyObject>();

		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			if (pathSegments.Length == 0)
			{
				return this;
			}
			return LocalHierarchy[pathSegments[0]].FollowPath(path.Remove(0, pathSegments[0].Length + 1));
		}
	}
}
