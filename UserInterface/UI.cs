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
		public static List<EditorWindow> EditorWindows = new List<EditorWindow>();

		private static void UI(ref ImaginaryHierarchyObject rootHierarchyObject, ref ImaginaryHierarchyObject currentSelectedHierarchyObject, Assembly userGeneratedCode)
		{
			MenuBar();
			DrawWindows();
		}

		private static void DrawWindows()
		{
			foreach (var window in EditorWindows)
			{
				window.UI();
			}
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
					if (ImGui.BeginMenu("New Window"))
					{
						foreach (var type in EditorWindowTypes)
						{
							if(ImGui.Button($"New {type.Name}"))
							{
								var window = (EditorWindow)Activator.CreateInstance(type);
								window.WindowTitle += EditorWindows.Count; // TODO: use something else for Unique Id generation! (perhaps use the RenderEngine2D system?)
								EditorWindows.Add(window);
							}
						}

						ImGui.EndMenu();
					}

					if (ImGui.BeginMenu("Open Windows"))
					{
						foreach (var window in EditorWindows)
						{
							bool enabled = window.Enabled;

							ImGui.Checkbox($"{window.WindowTitle} ({window.GetType().Name})", ref enabled);

							window.Enabled = enabled;
						}

						ImGui.EndMenu();
					}

					ImGui.EndMenu();
				}
			}
			ImGui.EndMainMenuBar();
		}
	}
}
