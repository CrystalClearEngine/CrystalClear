using System.Collections.Generic;

namespace CrystalClear.ECS
{
	public interface IEntity
	{
		public int EntityId { get; }

		public List<EntityAttribute> Attributes { get; set; }

		public void AttatchAttribute(EntityAttribute attribute)
		{
			Attributes.Add(attribute);
		}

		public void DetachAttribute(EntityAttribute attribute)
		{
			Attributes.Remove(attribute);
		}
	}
}
