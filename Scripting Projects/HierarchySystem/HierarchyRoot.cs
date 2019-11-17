using CrystalClear.HierarchySystem.Attributes;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	[HiddenHierarchyObject]
	public class HierarchyRoot : HierarchyObject/* : IHierarchyObjectManager*/
	{
		public HierarchyRoot() // Hmmm well I was gonna make a constructor with parameters and all, but I´ll settle for this as I don´t know what to add
		{
			
		}

		public new string Name
		{
			get
			{
				return HierarchySystem.GetName(this);
			}
		}

		private Dictionary<string, HierarchyObject> hierarchyObjects = new Dictionary<string, HierarchyObject>();
		public Dictionary<string, HierarchyObject> HierarchyObjects
		{
			get => hierarchyObjects;
		}

		public new Dictionary<string, HierarchyObject> LocalHierarchy => HierarchyObjects;


		public new HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			return HierarchyObjects[pathSegments[0]].FollowPath(path.Remove(0, pathSegments[0].Length + 1 /*+ 1 to make sure we remove the '/' from the path*/));
		}
	}
}
