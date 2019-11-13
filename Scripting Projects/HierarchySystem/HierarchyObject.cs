using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	interface IPathFollower
	{

	}

	public class HierarchyRoot : IPathFollower
	{


		public Dictionary<string, HierarchyObject> HierarchyObjects;
	}

	/// <summary>
	/// Represents any item that is part of the hierarchy
	/// </summary>
	public abstract class HierarchyObject : IPathFollower
	{
		public Dictionary<string, HierarchyObject> LocalHierarchy;
	}
}
