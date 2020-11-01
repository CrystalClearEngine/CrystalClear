using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CrystalClear.ScanningSystem
{
	public static class ScanningSystem
	{
		public static IEnumerable<Type> FindTypesWithAttribute(Type[] types, Type attributeType, bool checkInheritance)
		{
			foreach (var type in types)
			{
				if (type.GetCustomAttributes(attributeType, checkInheritance).Any((obj) => obj.GetType() == attributeType))
				{
					yield return type;
				}
			}
		}

		public static IEnumerable<Type> FindTypesWithAttribute<TAttribute>(Type[] types, bool checkInheritance)
		{
			foreach (var type in types)
			{
				if (type.GetCustomAttributes(typeof(TAttribute), checkInheritance).Any((obj) => obj.GetType() is TAttribute))
				{
					yield return type;
				}
			}
		}

		public static Type[] FindSubclasses()
		{

		}
	}
}
