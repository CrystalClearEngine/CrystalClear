using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using EditorMain;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using static CrystalClear.CrystalClearInformation;
using static CrystalClear.EditorInformation;
using static CrystalClear.Input;

namespace CrystalClear.UserInterface
{
	public static partial class UserInterface
	{
		static bool enableConsoleWindow = true;
		static string consoleInput = string.Empty;
		static string consoleLog = string.Empty;
		private static void ConsoleWindow()
		{
			if (!enableConsoleWindow)
			{
				return;
			}

			ImGui.Begin("ConsoleWindow");
			{
				ImGui.TextWrapped(consoleLog);

				ImGui.InputText("", ref consoleInput, 100);

				ImGui.SameLine();

				if (ImGui.Button("Send"))
				{
					Output.Log(consoleInput);

					MainClass.ParseCommand(consoleInput, ref rootHierarchyObject, ref currentSelectedHierarchyObject, userAssembly);

					consoleInput = string.Empty;
				}
			}
			ImGui.End();
		}
	}
}