using CrystalClear.ECS;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObject
		: IEntity
	{
		// Mixed/Hybrid ECS.
		public int EntityId { get; }

		public int ParentEntityId { get => Parent.EntityId; }

		public List<DataAttribute> Attributes { get; set; }
	}
}
