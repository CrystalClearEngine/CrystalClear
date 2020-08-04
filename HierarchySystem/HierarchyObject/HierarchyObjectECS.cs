using System.Collections.Generic;
using CrystalClear.ECS;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObject
		: IEntity
	{
		public int ParentEntityId => Parent.EntityId;

		// Mixed/Hybrid ECS.
		public int EntityId { get; }

		public List<DataAttribute> Attributes { get; set; }
	}
}