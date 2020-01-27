using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalClear
{
	public static class Utilities
	{
		public static string EnsureUniqueName(string currentName, IEnumerable<string> enumerable)
		{
			// Create iterator.
			int i = 1;

			// Repeat while name is already taken in AttatchedScripts.
			while (enumerable.Contains(currentName))
			{
				string OldDuplicateDecorator = $" ({i - 1})";

				// Does the name already contain the OldDuplicateDecorator from a previous attempt?
				if (currentName.EndsWith(OldDuplicateDecorator))
				{
					currentName =  currentName.Remove(currentName.Length - OldDuplicateDecorator.Length, OldDuplicateDecorator.Length);
				}

				string DuplicateDecorator = $" ({i})";

				currentName += DuplicateDecorator;

				// Increment iterator.
				i++;
			}

			return currentName;
		}
	}
}
