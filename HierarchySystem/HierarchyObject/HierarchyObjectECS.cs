using System.Collections.Generic;
using CrystalClear.ECS;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObject
	{
		public int ParentEntityId => Parent.EntityId;

		// Mixed/Hybrid ECS.
		public int EntityId { get; }
	}
}