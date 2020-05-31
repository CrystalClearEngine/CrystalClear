using CrystalClear.ECS;
using System;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem.ECS
{
	/// <summary>
	/// ECS entities that can fit into any ordinary Hierarchy.
	/// </summary>
	public struct HierarchyEntity
		: IEntity
		, IEquatable<HierarchyEntity>
	{
		public int EntityId { get; }

		public readonly int ParentEntityId;

		public readonly Hierarchy<HierarchyEntity> LocalEntityHierarchy;

		public readonly Hierarchy LocalHierarchy;

		#region Equality Overrides
		public override bool Equals(object obj)
		{
			return obj is HierarchyEntity entity && Equals(entity);
		}

		public bool Equals(HierarchyEntity other)
		{
			return EntityId == other.EntityId &&
				   ParentEntityId == other.ParentEntityId &&
				   LocalEntityHierarchy.Equals(other.LocalEntityHierarchy) &&
				   EqualityComparer<Hierarchy>.Default.Equals(LocalHierarchy, other.LocalHierarchy);
		}

		public override int GetHashCode()
		{
			var hashCode = -2012399249;
			hashCode = hashCode * -1521134295 + EntityId.GetHashCode();
			hashCode = hashCode * -1521134295 + ParentEntityId.GetHashCode();
			hashCode = hashCode * -1521134295 + LocalEntityHierarchy.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<Hierarchy>.Default.GetHashCode(LocalHierarchy);
			return hashCode;
		}

		public static bool operator ==(HierarchyEntity left, HierarchyEntity right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(HierarchyEntity left, HierarchyEntity right)
		{
			return !(left == right);
		}
		#endregion
	}
}