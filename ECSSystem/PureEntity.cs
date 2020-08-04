using System;
using System.Collections.Generic;

namespace CrystalClear.ECS
{
	/// <summary>
	///     The most basic form of Entity. A PureEntity only stores it's EntityId.
	/// </summary>
	public struct PureEntity
		: IEntity
			, IEquatable<PureEntity>
	{
		public int EntityId { get; }
		public List<DataAttribute> Attributes { get; set; }

		#region Equality Overrides

		public override bool Equals(object obj) => obj is PureEntity entity && Equals(entity);

		public bool Equals(PureEntity other) => EntityId == other.EntityId;

		public override int GetHashCode() => -1619204625 + EntityId.GetHashCode();

		public static bool operator ==(PureEntity left, PureEntity right) => left.Equals(right);

		public static bool operator !=(PureEntity left, PureEntity right) => !(left == right);

		#endregion
	}
}