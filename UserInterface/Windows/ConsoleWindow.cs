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
using static CrystalClear.UserInterface.UserInterface;


namespace CrystalClear.UserInterface
{
	public class ConsoleWindow : EditorWindow
	{
		public ConsoleWindow()
		{
			// TODO: remember to unsub!
			Output.OutputLogged += (string newLog) => consoleLog += newLog + Environment.NewLine;
		}

		public override string WindowTitle { get; protected set; } = "Console Window";

		protected override void UIImpl()
		{
			PlayControlsUI();
		}

		private string consoleLog = string.Empty;
		private string consoleInput = string.Empty;

		public void PlayControlsUI()
		{
			ImGui.TextWrapped(consoleLog);

			ImGui.InputText("", ref consoleInput, 100);

			ImGui.SameLine();

			if (ImGui.Button("Send"))
			{
				Output.Log(consoleInput);

				MainClass.ParseCommand(consoleInput, ref RootHierarchyObject, ref CurrentSelectedHierarchyObject, UserAssembly);

				consoleInput = string.Empty;
			}
		}
	}
}