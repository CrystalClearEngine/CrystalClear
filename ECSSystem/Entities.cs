using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.ECS;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CrystalClear.ECSSystem
{
	// TODO: add getter for this in EntityBase!
	public enum EntityType
	{
		PureEntity,
		HierarchyEntity,
		HierarchyObject,
	}

	// TODO: turn EntityBase into IEntity interface instead?
	public struct EntityBase : IEquatable<EntityBase>
	{
		public EntityBase(PureEntity pureEntity)
		{
			PureEntity = pureEntity;
			HierarchyEntity = null;
			HierarchyObject = null;
		}

		public EntityBase(HierarchyEntity hierarchyEntity)
		{
			PureEntity = null;
			HierarchyEntity = hierarchyEntity;
			HierarchyObject = null;
		}

		public EntityBase(HierarchyObject hierarchyObject)
		{
			PureEntity = null;
			HierarchyEntity = null;
			HierarchyObject = hierarchyObject;
		}

		public int EntityId
		{
			get
			{
				if (PureEntity.HasValue)
				{
					return PureEntity.Value.EntityId;
				}

				if (HierarchyEntity.HasValue)
				{
					return HierarchyEntity.Value.EntityId;
				}

				if (!(HierarchyObject is null))
				{
					return HierarchyObject.EntityId;
				}

				throw new Exception("This EntityBase has no entities.");
			}
		}

		public readonly PureEntity? PureEntity;

		public readonly HierarchyEntity? HierarchyEntity;

		public readonly HierarchyObject HierarchyObject;

		#region Equality Overrides
		public override bool Equals(object obj)
		{
			return obj is EntityBase @base && Equals(@base);
		}

		public bool Equals(EntityBase other)
		{
			return EntityId == other.EntityId &&
				   EqualityComparer<PureEntity?>.Default.Equals(PureEntity, other.PureEntity) &&
				   EqualityComparer<HierarchyEntity?>.Default.Equals(HierarchyEntity, other.HierarchyEntity) &&
				   EqualityComparer<HierarchyObject>.Default.Equals(HierarchyObject, other.HierarchyObject);
		}

		public override int GetHashCode()
		{
			var hashCode = -1426568347;
			hashCode = hashCode * -1521134295 + EntityId.GetHashCode();
			hashCode = hashCode * -1521134295 + PureEntity.GetHashCode();
			hashCode = hashCode * -1521134295 + HierarchyEntity.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<HierarchyObject>.Default.GetHashCode(HierarchyObject);
			return hashCode;
		}

		public static bool operator ==(EntityBase left, EntityBase right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(EntityBase left, EntityBase right)
		{
			return !(left == right);
		}
		#endregion
	}

	/// <summary>
	/// The most basic form of Entity. A PureEntity only stores it's EntityId.
	/// </summary>
	public struct PureEntity : IEquatable<PureEntity>
	{
		public readonly int EntityId;

		#region Equality Overrides
		public override bool Equals(object obj)
		{
			return obj is PureEntity entity && Equals(entity);
		}

		public bool Equals(PureEntity other)
		{
			return EntityId == other.EntityId;
		}

		public override int GetHashCode()
		{
			return -1619204625 + EntityId.GetHashCode();
		}

		public static bool operator ==(PureEntity left, PureEntity right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(PureEntity left, PureEntity right)
		{
			return !(left == right);
		}
		#endregion
	}
}
