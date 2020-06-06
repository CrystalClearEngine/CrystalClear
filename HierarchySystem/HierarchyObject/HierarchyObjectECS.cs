using CrystalClear.ECS;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObjectProperties
		: IEntity
	{
		// Mixed/Hybrid ECS.
		public int EntityId { get; }
		public int ParentEntityId { get => Parent.EntityId; }
	}
}
