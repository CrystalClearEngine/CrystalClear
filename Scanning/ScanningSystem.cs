using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace CrystalClear.ScanningSystem
{
	public static class ScanningSystem
	{
		public static IEnumerable<Type> TypesInAssemblies(IEnumerable<Assembly> assemblies)
		{
			foreach (var assembly in assemblies)
			{
				foreach (var type in assembly.GetTypes())
				{
					yield return type;
				}
			}
		}

		public static IEnumerable<Type> FindTypesWithAttribute(IEnumerable<Type> types, Type attributeType, bool checkInheritance)
		{
			foreach (var type in types)
			{
				if (type.GetCustomAttributes(attributeType, checkInheritance).Length > 0)
				{
					yield return type;
				}
			}
		}

		public static IEnumerable<Type> FindTypesWithAttribute<TAttribute>(IEnumerable<Type> types, bool checkInheritance)
		{
			foreach (var type in types)
			{
				if (type.GetCustomAttributes(typeof(TAttribute), checkInheritance).Length > 0)
				{
					yield return type;
				}
			}
		}

		public static IEnumerable<Type> FindSubclasses(IEnumerable<Type> types, Type baseClass)
		{
			foreach (var type in types)
			{
				if (type.IsSubclassOf(baseClass))
				{
					yield return type;
				}
			}
		}

		public static IEnumerable<Type> FindSubclasses<TBase>(IEnumerable<Type> types)
		{
			foreach (var type in types)
			{
				if (type.IsSubclassOf(typeof(TBase)))
				{
					yield return type;
				}
			}
		}
	}
}
