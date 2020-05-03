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

	public static class ConsoleInput
	{
		public static bool AskYOrNQuestion(string question)
		{
			retry:
			Console.Write(question + ": ");

			switch (Console.ReadKey().KeyChar)
			{
				case 't':
				case 'y':
					Console.WriteLine();
					return true;

				case 'f':
				case 'n':
					Console.WriteLine();
					return false;

				default:
					Console.WriteLine("Invalid!");
					goto retry;
			}
		}

		public static string AskQuestion(string question)
		{
			Console.Write(question + ": ");
			string response = Console.ReadLine();
			return response;
		}
	}
}
