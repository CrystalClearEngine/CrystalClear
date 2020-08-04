using System.Collections.Generic;

namespace CrystalClear.ECS
{
	public abstract class SelectiveECSSystem : ECSSystem
	{
		private readonly List<int> entitiesUnderControl = new List<int>();

		protected IEnumerable<IEntity> EnumerateEntities()
		{
			lock (entitiesUnderControl)
				foreach (var id in entitiesUnderControl)
					yield return GetEntity(id);
		}

		protected void AddControlledEntity(IEntity entity)
		{
			AddControlledEntity(entity.EntityId);
		}

		protected void AddControlledEntity(int entityId)
		{
			lock (entitiesUnderControl)
				entitiesUnderControl.Add(entityId);
		}

		protected void UncontrolEntity(IEntity entity)
		{
			UncontrolEntity(entity.EntityId);
		}

		protected void UncontrolEntity(int entityId)
		{
			lock (entitiesUnderControl)
				entitiesUnderControl.Remove(entityId);
		}

		public bool Controls(IEntity entity) => Controls(entity.EntityId);

		public bool Controls(int entityId) => entitiesUnderControl.Contains(entityId);
	}
}