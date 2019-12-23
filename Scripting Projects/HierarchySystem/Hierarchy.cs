using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.HierarchySystem
{
	[Serializable]
	public class Hierarchy // TODO maybe make this generic and allow different types of Hierarchies to be formed from this? Should in that case also be in ScriptUtilities...
	{
		private Dictionary<string, HierarchyObject> hierarchy;
		public HierarchyObject this[string index]
		{
			get => hierarchy[index];
			set => hierarchy[index] = value;
		}
		public event Action OnHierarchyChange;
	}
}
