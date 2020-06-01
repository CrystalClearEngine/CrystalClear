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
			if (question.EndsWith(":") || question.EndsWith("?"))
			{
				Console.Write(question + " ");
			}
			else
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
					Console.WriteLine("\nInvalid, should be 't' or 'y' for true and 'f' or 'n' for false!");
					goto retry;
			}
		}

		public static string AskQuestion(string question)
		{
			if (question.EndsWith(":") || question.EndsWith("?"))
			{
				Console.Write(question + " ");
			}
			else
				Console.Write(question + ": ");
			string response = Console.ReadLine();
			return response;
		}
	}
}
