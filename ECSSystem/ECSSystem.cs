using System.Collections.Generic;

namespace CrystalClear.ECS
{
	public abstract class ECSSystem
	{
		// TODO: Use uint instead, since there is not really any reason to use negative values in ids?
		private static Dictionary<int, IEntity> allEntities = new Dictionary<int, IEntity>();

		public static List<ECSSystem> ECSSystems = new List<ECSSystem>();

		public static IEntity GetEntity(int id)
		{
			lock (allEntities)
				return allEntities[id];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="startingId"></param>
		/// <param name="endingId"></param>
		/// <param name="lockNotCopy">By default the method will copy the AllEntities array before </param>
		/// <returns></returns>
		public static IEntity[] GetEntities(int startingId, int endingId, bool lockNotCopy = false)
		{
			// TODO: do performance profiling to see what is fastest (lock or copy), and if lockNotCopy even needs to be an option.
			List<IEntity> entityBases = new List<IEntity>();

			if (lockNotCopy)
			{
				lock (allEntities)
				{
					foreach (int id in allEntities.Keys)
					{
						if (id >= startingId && id <= endingId)
						{
							entityBases.Add(allEntities[id]);
						}
					}
				}
			}
			else
			{
				Dictionary<int, IEntity> copyOfAllEntities;

				lock (allEntities)
					copyOfAllEntities = new Dictionary<int, IEntity>(allEntities);

				foreach (int id in copyOfAllEntities.Keys)
				{
					if (id >= startingId && id <= endingId)
					{
						entityBases.Add(copyOfAllEntities[id]);
					}
				}
			}

			return entityBases.ToArray();
		}

		public static void AddEntity(IEntity entity)
		{
			lock (allEntities)
				allEntities.Add(entity.EntityId, entity);
		}

		public static void DeleteEntity(int id)
		{
			lock (allEntities)
				allEntities.Remove(id);
		}
	}
}
