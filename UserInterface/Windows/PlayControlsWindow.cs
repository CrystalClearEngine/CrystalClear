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
	public class PlayControlsWindow : EditorWindow
	{
		public override string WindowTitle { get; protected set; } = "Controls";

		protected override void UIImpl()
		{
			PlayControlsUI();
		}

		public void PlayControlsUI()
		{
			ImGui.SetWindowSize(new System.Numerics.Vector2(80, 60), ImGuiCond.Always);

			if (ImGui.Button(RuntimeMain.RuntimeMain.IsRunning ? "[]" : ">"))
			{
				if (RuntimeMain.RuntimeMain.IsRunning)
				{
					// Stop.
					RuntimeMain.RuntimeMain.Stop();
				}
				else
				{
					// Start.
					Task.Run(() => MainClass.Run(RootHierarchyObject, UserAssembly));
				}
			}

			ImGui.SameLine();

			if (ImGui.Button("||"))
			{
				// Pause
			}
		}
	}
}