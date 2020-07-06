﻿using System;
using System.Collections;

namespace CrystalClear
{
	// TODO: maybe rename to DeveloperConsole?
	public static class Output
	{
		public static void Log()
		{
			Output.Log();
		}

		/// <summary>
		/// Write an array of objects to the console or log.
		/// </summary>
		/// <param name="objs">The objects to write.</param>
		public static void Log(object[] objs)
		{
			foreach (object obj in objs)
			{
				Output.Log(obj);
			}
		}

		/// <summary>
		/// Write a collection of objects to the console or log.
		/// </summary>
		/// <param name="objs">The objects to write.</param>
		public static void Log(ICollection objs)
		{
			foreach (object obj in objs)
			{
				Output.Log(obj);
			}
		}

		/// <summary>
		/// Write an object to the console or log.
		/// </summary>
		/// <param name="obj">The object to write.</param>
		public static void Log(object obj)
		{
			Output.Log(obj);
		}

		/// <summary>
		/// Write a string to the console or log.
		/// </summary>
		/// <param name="str">The string to write.</param>
		public static void Log(string str)
		{
			Output.Log(str);
		}

		/// <summary>
		/// Writes a string to the output with additional settings for background color and foreground color.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="bgColor"></param>
		/// <param name="fgColor"></param>
		public static void Log(string str, ConsoleColor bgColor, ConsoleColor fgColor)
		{
			ConsoleColor prevFgColor = Console.ForegroundColor; // Store previous foreground and background color so that we can restore them after writing.
			ConsoleColor prevBgColor = Console.BackgroundColor;
			Console.BackgroundColor = bgColor; // Set new colors.
			Console.ForegroundColor = fgColor;
			Output.Log(str); // Write string.
			Console.BackgroundColor = prevBgColor; // Restore previous colors.
			Console.ForegroundColor = prevFgColor;
		}

		public static void ErrorLog(string str, bool minorError = true)
		{
			ConsoleColor prevFgColor = Console.ForegroundColor; // Store previous foreground and background color so that we can restore them after writing.
			Console.ForegroundColor = minorError ? ConsoleColor.DarkYellow : ConsoleColor.Red; // Set new colors.
			Output.Log(str); // Write string.
			Console.ForegroundColor = prevFgColor; // Restore previous colors.
		}
	}
}