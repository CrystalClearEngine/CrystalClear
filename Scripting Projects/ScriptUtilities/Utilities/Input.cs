using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
