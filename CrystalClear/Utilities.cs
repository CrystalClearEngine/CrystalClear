using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrystalClear
{
	public static class Utilities
	{
		public static string EnsureUniqueName(string baseName, IEnumerable<string> enumerable)
		{
			// Create iterator.
			int i = 1;

			// Repeat while name is already taken in AttatchedScripts.
			while (enumerable.Contains(baseName))
			{
				string OldDuplicateDecorator = $" ({i - 1})";

				// Does the name already contain the OldDuplicateDecorator from a previous attempt?
				if (baseName.EndsWith(OldDuplicateDecorator))
				{
					baseName = baseName.Remove(baseName.Length - OldDuplicateDecorator.Length, OldDuplicateDecorator.Length);
				}

				string DuplicateDecorator = $" ({i})";

				baseName += DuplicateDecorator;

				// Increment iterator.
				i++;
			}

			return baseName;
		}

		public static bool ReflectionEquals(this object a, object b)
		{
			// A and b cannot be compared if their types are not compatible.
			if (b.GetType().IsAssignableFrom(a.GetType()))
			{
				//b = Convert.ChangeType(b, a.GetType()); // WORKING ON ACTUALLY ALLOWING ASSIGNABLES TO BE COMPREAD
				return false;
			}
			// We know for sure that we don't need to do any other checking if a and b are the same instances.
			else if (ReferenceEquals(a, b))
			{
				return true;
			}

			FieldInfo[] fields = a.GetType().GetFields();
			PropertyInfo[] properties = a.GetType().GetProperties();

			// Iterate through all fields to compare them.
			for (int i = 0; i < fields.Length; i++)
			{
				// Store field A and B in variables.
				object fieldA = fields[i].GetValue(a);
				object fieldB = fields[i].GetValue(b);

				// Is fieldA (and fieldB as well, since they are of the same type) primitive or Equatable? If so, we can compare them!
				if (fieldA.GetType().IsPrimitive || fieldA.GetType().GetInterfaces().Any(x =>
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
						continue;
				}

				// Are the A and B fields at this index non-(reflectively)equal?
				else if (!ReflectionEquals(fieldA, fieldB))
				{
					// Return the method with false, as no other checks need to be done if even just one field is different.
					return false;
				}

				// If A and B are equal, we can continue.
			}

			// Iterate through all properties to compare them.
			for (int i = 0; i < properties.Length; i++)
			{
				// Store property A and B in variables.
				object propertyA = properties[i].GetValue(a);
				object propertyB = properties[i].GetValue(b);

				// Is propertyA (and propertyB as well, since they are of the same type) primitive or Equatable? If so, we can compare them!
				if (propertyA.GetType().IsPrimitive || propertyA.GetType().GetInterfaces().Any(x =>
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
						continue;
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
	}
}
