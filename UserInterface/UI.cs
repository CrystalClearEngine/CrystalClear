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
		private static void UI(ref ImaginaryHierarchyObject rootHierarchyObject, ref ImaginaryHierarchyObject currentSelectedHierarchyObject, Assembly userGeneratedCode)
		{
			MenuBar();
			HierarchyViewer(rootHierarchyObject);
			Modifier(ref currentSelectedHierarchyObject);
			ConsoleWindow();
			PlayControls();
			AssetExplorer();
		}

		private static void MenuBar()
		{
			ImGui.BeginMainMenuBar();
			{
				if (ImGui.BeginMenu("Project"))
				{
					if (ImGui.MenuItem("Save Project"))
					{
						ProjectInfo.SaveCurrentProject();
					}

					if (ImGui.MenuItem("Build"))
					{
						CompilationSystem.Builder.Build(CurrentProject.BuildPath, "build");
					}

					ImGui.EndMenu();
				}

				if (ImGui.BeginMenu("Windows"))
				{
					ImGui.Checkbox("ConsoleWindow", ref enableConsoleWindow);

					ImGui.Checkbox("Modifier", ref enableModifier);

					ImGui.Checkbox("Hierarchy Viewer", ref enableHierarchyViewer);

					ImGui.Checkbox("Asset Explorer", ref enableAssetExplorer);

					ImGui.EndMenu();
				}
			}
			ImGui.EndMainMenuBar();
		}
	}
}
