using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using CrystalClear;

public static partial class MainClass
{
	// TODO: rename to UserGeneratedCode?
	private static Assembly compiledAssembly;

	private static void Main()
	{
		Console.ForegroundColor = ConsoleColor.White;
		Console.BackgroundColor = ConsoleColor.Black;

		#region Thread Culture

#if DEBUG
		Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US"); // To ensure google-able exceptions.
#endif

		#endregion

		while (true)
		{
			Output.Clear();
			ProjectSelect();
			Editor();
		}
	}
}