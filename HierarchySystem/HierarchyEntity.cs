using CrystalClear.ECS;

namespace CrystalClear.HierarchySystem.ECS
{
	/// <summary>
	/// ECS entities that can fit into any ordinary Hierarchy.
	/// </summary>
	public struct HierarchyEntity
		: IEntity
	{
		public int EntityId { get; }

		public readonly int ParentEntityId;

		public readonly Hierarchy<HierarchyEntity> LocalEntityHierarchy;

		public readonly Hierarchy LocalHierarchy;
	}
}