using System.Collections.Generic;

namespace CrystalClear.ECS
{
	public interface IEntity
	{
		public int EntityId { get; }

		public List<DataAttribute> Attributes { get; set; }

		public void AttachedAttribute(DataAttribute attribute)
		{
			Attributes.Add(attribute);
		}

		public void DetachAttribute(DataAttribute attribute)
		{
			Attributes.Remove(attribute);
		}
	}
}
