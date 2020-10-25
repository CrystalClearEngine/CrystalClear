using System;
using System.Collections;

namespace CrystalClear
{
	// TODO: maybe rename to DeveloperConsole?
	public static class Output
	{
		public static event Action<string> OutputLogged;

		// TODO: rename to NewLine?
		public static void Log()
		{
			OutputLogged?.Invoke("\n");
			Console.WriteLine();
		}

		/// <summary>
		///     Write an array of objects to the console or log.
		/// </summary>
		/// <param name="objs">The objects to write.</param>
		public static void Log(object[] objs)
		{
			foreach (object obj in objs)
			{
				OutputLogged?.Invoke(obj.ToString());
				Console.WriteLine(obj);
			}
		}

		/// <summary>
		///     Write a collection of objects to the console or log.
		/// </summary>
		/// <param name="objs">The objects to write.</param>
		public static void Log(ICollection objs)
		{
			foreach (object obj in objs)
			{
				OutputLogged?.Invoke(obj.ToString());
				Console.WriteLine(obj);
			}
		}

		/// <summary>
		///     Write an object to the console or log.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		public static void Log(object obj)
		{
			OutputLogged?.Invoke(obj.ToString());
			Console.WriteLine(obj);
		}

		/// <summary>
		///     Write a string to the console or log.
		/// </summary>
		/// <param name="str">The string to write.</param>
		public static void Log(string str)
		{
			OutputLogged?.Invoke(str);
			Console.WriteLine(str);
		}
		// TODO: visual artifacts can appear if multiple threads are Logging simultaneusly! Make sure only one logging operation can be performed at once. Make it async and store in queue before printing?
		/// <summary>
		///     Writes a string to the output with additional settings for background color and foreground color.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="bgColor"></param>
		/// <param name="fgColor"></param>
		public static void Log(string str, ConsoleColor bgColor, ConsoleColor fgColor)
		{
			OutputLogged?.Invoke(str);
			ConsoleColor
				prevFgColor =
					Console.ForegroundColor; // Store previous foreground and background color so that we can restore them after writing.
			ConsoleColor prevBgColor = Console.BackgroundColor;
			Console.BackgroundColor = bgColor; // Set new colors.
			Console.ForegroundColor = fgColor;
			Console.WriteLine(str); // Write string.
			Console.BackgroundColor = prevBgColor; // Restore previous colors.
			Console.ForegroundColor = prevFgColor;
		}

		public static void ErrorLog(string str, bool minorError = true)
		{
			OutputLogged?.Invoke(str);
			ConsoleColor
				prevFgColor =
					Console.ForegroundColor; // Store previous foreground and background color so that we can restore them after writing.
			Console.ForegroundColor = minorError ? ConsoleColor.DarkYellow : ConsoleColor.Red; // Set new colors.
			Console.WriteLine(str); // Write string.
			Console.ForegroundColor = prevFgColor; // Restore previous colors.
		}

		public static void Clear()
		{
			Console.Clear();
		}
	}
}