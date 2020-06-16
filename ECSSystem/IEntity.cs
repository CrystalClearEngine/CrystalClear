using System.Collections.Generic;

namespace CrystalClear.ECS
{
	public interface IEntity
	{
		public int EntityId { get; }

		public List<EntityDataAttribute> Attributes { get; set; }

		public void AttatchAttribute(EntityDataAttribute attribute)
		{
			Attributes.Add(attribute);
		}

		public void DetachAttribute(EntityDataAttribute attribute)
		{
			Attributes.Remove(attribute);
		}
	}
}
