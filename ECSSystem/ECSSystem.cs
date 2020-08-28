using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.ECS
{
	public class ECSSystem<TDataAttribute>
		where TDataAttribute : DataAttribute
	{
		public bool HasDataFor(uint entityID) => data.ContainsKey(entityID);

		public void Add(uint entityID, TDataAttribute dataAttribute) => data.Add(entityID, dataAttribute);

		public TDataAttribute this[uint entityID]
		{
			get => data[entityID];
			set => data[entityID] = value;
		}

		public Dictionary<uint, TDataAttribute> data = new Dictionary<uint, TDataAttribute>();

		public void ExecuteOnEach(Action<uint, TDataAttribute> action)
		{
			foreach (var entity in data)
			{
				action(entity.Key, entity.Value);
			}
		}

		public void ExecuteOnEachDataAttribute(Action<TDataAttribute, uint> action)
		{
			foreach (var entity in data)
			{
				action(entity.Value, entity.Key);
			}
		}
	}

	public class MultipleDataAttribute<TDataAttribute> : DataAttribute
		where TDataAttribute : DataAttribute
	{
		public List<TDataAttribute> DataAttributes = new List<TDataAttribute>();
	}
}
