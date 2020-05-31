using System.Collections.Generic;

namespace CrystalClear.ECSSystem
{
	public abstract class ECSSystem
	{
		// TODO: Use uint instead, since there is not really any reason to use negative values in ids?
		private static Dictionary<int, EntityBase> allEntities = new Dictionary<int, EntityBase>();

		public static List<ECSSystem> ECSSystems = new List<ECSSystem>();

		public static EntityBase GetEntity(int id)
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
		public static EntityBase[] GetEntities(int startingId, int endingId, bool lockNotCopy = false)
		{
			// TODO: do performance profiling to see what is fastest (lock or copy), and if lockNotCopy even needs to be an option.
			List<EntityBase> entityBases = new List<EntityBase>();

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
				Dictionary<int, EntityBase> copyOfAllEntities;

				lock (allEntities)
					copyOfAllEntities = new Dictionary<int, EntityBase>(allEntities);

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

		public static void AddEntity(EntityBase entity)
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
