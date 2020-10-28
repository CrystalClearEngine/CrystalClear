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
		private static void PlayControls()
		{
			ImGui.Begin("Controls");
			{
				ImGui.SetWindowSize(new System.Numerics.Vector2(80, 60), ImGuiCond.Always);

				if (ImGui.Button(RuntimeMain.RuntimeMain.IsRunning ? "[]" : ">"))
				{
					if (RuntimeMain.RuntimeMain.IsRunning)
					{
						// Stop
					}
					else
					{
						Task.Run(() => MainClass.Run(rootHierarchyObject, userAssembly));
					}
				}

				ImGui.SameLine();

				if (ImGui.Button("||"))
				{
					// Pause
				}
				
			} ImGui.End();
		}
	}
}