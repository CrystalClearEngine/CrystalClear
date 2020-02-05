using System.Collections.Generic;
using System.Linq;

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
	}
}
