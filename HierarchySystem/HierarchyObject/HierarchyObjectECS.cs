using System.Collections.Generic;
using System.Linq;
using CrystalClear.ECS;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObject
		: IEntity
	{
		public int ParentEntityId => Parent.EntityId;

		// Mixed/Hybrid ECS.
		public int EntityId { get; }

		// TODO: make sorted dictionary!
		public List<DataAttribute> Attributes { get; set; }

		public T GetAttribute<T>()
			where T : DataAttribute
		{
			return (T)Attributes.First((attribute) => attribute is T);
		}

		public T[] GetAllAttributes<T>()
			where T : DataAttribute
		{
			return (T[])Attributes.Where((attribute) => attribute is T).ToArray();
		}
	}
}