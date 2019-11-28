using System;

namespace CrystalClear.ScriptUtilities
{
	public static class Output
	{
		public static void Log(object[] objs)
		{
			foreach (object obj in objs)
			{
				Console.WriteLine(obj);
			}
		}

		public static void Log(object obj)
		{
			Console.WriteLine(obj);
		}

		public static void Log(string str)
		{
			Console.WriteLine(str);
		}

		public static void Log(string str, ConsoleColor bgColor, ConsoleColor fgColor)
		{
			ConsoleColor
				prevFgColor =
					Console.ForegroundColor; // Store previous foreground and background color so that we can restore them after writing
			ConsoleColor prevBgColor = Console.BackgroundColor;
			Console.BackgroundColor = bgColor; // Set new colors
			Console.ForegroundColor = fgColor;
			Console.WriteLine(str); // Write string
			Console.BackgroundColor = prevBgColor; // Restore previous colors
			Console.ForegroundColor = prevFgColor;
		}
	}
}