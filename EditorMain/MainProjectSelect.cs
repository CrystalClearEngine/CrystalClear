using System;
using CrystalClear;
using static CrystalClear.Input;

partial class MainClass
{
	private static void ProjectSelect()
	{
		#region Project Selection

		Output.Log("Please open or create a new project:");
		ProjectSelection:
		switch (Console.ReadLine())
		{
			case "new":
				ProjectInfo.NewProject(AskQuestion("Pick a path for the new project"),
					AskQuestion("Pick a name for the new project"));
				break;

			case "open":
				try
				{
					ProjectInfo.OpenProject(AskQuestion("Enter the path of the project"));
				}
				catch (ArgumentException)
				{
					goto ProjectSelection;
				}

				break;

			default:
				Output.ErrorLog("command error: unknown command");
				goto ProjectSelection;
		}

		#endregion
	}
}