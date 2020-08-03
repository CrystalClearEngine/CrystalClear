using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CrystalClear
{
	public static class Utilities
	{
		public static void CreateEmptyFile(string path)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.Create(path).Dispose();
		}

		/// <summary>
		/// Returns a name that is ensured to be unique among the provided names and based on a base name.
		/// Example:
		/// The name "QWERTY" was attemped to be added into a group of names already containing the name "QWERTY".
		/// "QWERTY" is passed as the baseName and the group of other names as otherNames.
		/// "QWERTY" is renamed to "QWERTY (1)", and if another "QUERTY" is attempted to be added it is changed to "QUERTY (2)" etc.
		/// </summary>
		/// <param name="baseName">The name to use as a base for the new name.</param>
		/// <param name="otherNames">An enumerable of all other names </param>
		/// <returns>A unique name, based on the baseName and unique among otherNames.</returns>
		public static string EnsureUniqueName(string baseName, IEnumerable<string> otherNames)
		{
			int i = 1;

			// Repeat while name is already taken in AttachedScripts.
			while (otherNames.Contains(baseName))
			{
				string OldDuplicateDecorator = $" ({i - 1})";

				// Does the name already contain the OldDuplicateDecorator from a previous attempt?
				if (baseName.EndsWith(OldDuplicateDecorator))
				{
					baseName = baseName.Remove(baseName.Length - OldDuplicateDecorator.Length, OldDuplicateDecorator.Length);
				}

				string DuplicateDecorator = $" ({i})";

				baseName += DuplicateDecorator;

				i++;
			}

			return baseName;
		}

		/// <summary>
		/// Compares two objects using reflection to find fields and properties.
		/// </summary>
		/// <param name="includePrivate">Whether to include private properties and fields when comparing the two objects.</param>
		/// <param name="ignoreProperties">Whether to include properties when comparing the two objects.</param>
		/// <returns>Whether the objects are equal.</returns>
		public static bool ReflectionEquals(this object a, object b, bool includePrivate = false, bool ignoreProperties = false)
		{
			if (a is null && b is null) // null is null
			{
				return true;
			}

			else if (a is null || b is null) // if both aren't, then if one is they can´t be equal
			{
				return false;
			}

			Type aType = a.GetType();
			Type bType = b.GetType();

			// A and B cannot be compared if their types are not compatible.
			if (!aType.IsAssignableFrom(bType) || aType.AssemblyQualifiedName != bType.AssemblyQualifiedName) // TODO: maybe add xType.IsSubclassOf?
			{
				//b = Convert.ChangeType(b, aType); // TODO: WORKING ON ACTUALLY ALLOWING ASSIGNABLES TO BE COMPREAD.
				return false;
			}

			// We know for sure that we don't need to do any other checking if a and b are the same instances.
			else if (ReferenceEquals(a, b))
			{
				return true;
			}

			FieldInfo[] fields;
			PropertyInfo[] properties = Array.Empty<PropertyInfo>();

			if (includePrivate)
			{
				fields = aType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (!ignoreProperties)
				{
					properties = aType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				}
			}
			else
			{
				fields = aType.GetFields();
				if (!ignoreProperties)
				{
					properties = aType.GetProperties();
				}
			}

			for (int i = 0; i < fields.Length; i++)
			{
				object fieldA = fields[i].GetValue(a);
				object fieldB = fields[i].GetValue(b);

				Type fieldAType = fieldA.GetType();

				// Is fieldA (and fieldB as well, since they are of the same type) primitive or Equatable? If so, we can compare them!
				if (fieldAType.IsPrimitive || fieldAType.GetInterfaces().Any(x =>
						x.IsGenericType &&
						x.GetGenericTypeDefinition() == typeof(IEquatable<>)))
				{
					// Are the A and B fields at this index non-equal?
					if (!fieldA.Equals(fieldB))
					{
						// Return the method with false, as no other checks need to be done if even just one field is different.
						return false;
					}
					else
					{
						continue;
					}
				}

				// Are the A and B fields at this index non-(reflectively)equal?
				else if (!ReflectionEquals(fieldA, fieldB))
				{
					// Return the method with false, as no other checks need to be done if even just one field is different.
					return false;
				}

				// If A and B are equal, we can continue.
			}

			for (int i = 0; i < properties.Length; i++)
			{
				object propertyA = properties[i].GetValue(a);
				object propertyB = properties[i].GetValue(b);

				Type propertyAType = propertyA.GetType();

				// Is propertyA (and propertyB as well, since they are of the same type) primitive or Equatable? If so, we can compare them!
				if (propertyAType.IsPrimitive || propertyAType.GetInterfaces().Any(x =>
						x.IsGenericType &&
						x.GetGenericTypeDefinition() == typeof(IEquatable<>)))
				{
					// Are the A and B properties at this index non-equal?
					if (!propertyA.Equals(propertyB))
					{
						// Return the method with false, as no other checks need to be done if even just one property is different.
						return false;
					}
					else
					{
						continue;
					}
				}

				// Are the A and B properties at this index non-(reflectively)equal?
				else if (!ReflectionEquals(propertyA, propertyB))
				{
					// Return the method with false, as no other checks need to be done if even just one property is different.
					return false;
				}

				// If A and B are equal, we can continue.
			}

			return true;
		}

		public static T TryGetTargetExt<T>(this WeakReference<T> weakReference)
			where T : class
		{
			weakReference.TryGetTarget(out T value);

			return value;
		}
	}
}
