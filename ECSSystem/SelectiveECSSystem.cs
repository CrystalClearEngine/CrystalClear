using System.Collections.Generic;

namespace CrystalClear.ECSSystem
{
	public abstract class SelectiveECSSystem : ECSSystem
	{
		private List<int> entitiesUnderControl = new List<int>();

		protected IEnumerable<EntityBase> EnumerateEntities()
		{
			lock (entitiesUnderControl)
				foreach (int id in entitiesUnderControl)
					yield return ECSSystem.GetEntity(id);
		}

		protected void AddControlledEntity(EntityBase entity) => AddControlledEntity(entity.EntityId);

		protected void AddControlledEntity(int entityId)
		{
			lock (entitiesUnderControl)
				entitiesUnderControl.Add(entityId);
		}

		protected void UncontrolEntity(EntityBase entity) => UncontrolEntity(entity.EntityId);

		protected void UncontrolEntity(int entityId)
		{
			lock (entitiesUnderControl)
				entitiesUnderControl.Remove(entityId);
		}

		public bool Controls(EntityBase entity) => Controls(entity.EntityId);

		public bool Controls(int entityId) => entitiesUnderControl.Contains(entityId);
	}
}

