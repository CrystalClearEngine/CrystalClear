using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting.ScriptingEngine
{
	public static class Output
	{
		public static void Log(string str)
		{
			Console.WriteLine(str);
		}
		public static void Log(string str, ConsoleColor bgColor, ConsoleColor fgColor)
		{
			var prevFgColor = Console.ForegroundColor; //Store previous foreground and background color so that we can restore them after writing
			var prevBgColor = Console.BackgroundColor;
			Console.BackgroundColor = bgColor; //Set new colors
			Console.ForegroundColor = fgColor;
			Console.WriteLine(str); //Write string
			Console.BackgroundColor = prevBgColor; //Restore previous colors
			Console.ForegroundColor = prevFgColor;
		}
	}
}
