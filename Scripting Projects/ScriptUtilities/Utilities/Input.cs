using System;

namespace CrystalClear.ScriptUtilities.Utilities
{
	public class Input
	{
		public void WaitForKey(ConsoleKey key)
		{
			do
			{
				if (Console.ReadKey().Key == key)
				{
					return;
				}
			}
			while (true);
		}
	}
}
