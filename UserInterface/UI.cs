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

					ImGui.EndMenu();
				}
			}
			ImGui.EndMainMenuBar();
		}

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

		// TODO: make general purpose.
		static bool enableModifier = true;
		private static void Modifier(ref ImaginaryHierarchyObject currentSelectedHierarchyObject /*TODO: to pass this, or not to pass this (use static), that is the question. Please make sure it is consistent in this file.*/)
		{
			if (!enableModifier)
			{
				return;
			}

			ImGui.Begin("Modifier");
			{
				if (currentSelectedHierarchyObject is null)
				{
					ImGui.Text("Nothing selected.");
					return;
				}

				string name = currentSelectedHierarchyObject.Name;
				ImGui.InputText("Name", ref name, 100);
				currentSelectedHierarchyObject.Name = name;

				if (ImGui.Button("Delete"))
				{
					MainClass.DeleteHierarchyObject(currentSelectedHierarchyObject.Parent, currentSelectedHierarchyObject.Name);
					currentSelectedHierarchyObject = null;
					return;
				}

				if (ImGui.CollapsingHeader("Attatched Scripts"))
				{
					foreach (var script in currentSelectedHierarchyObject.AttachedScripts)
					{
						ModifyScript(script.Key, script.Value);
					}

					if (ImGui.Button("Attatch Script"))
					{
						MainClass.AddScript(currentSelectedHierarchyObject, new ImaginaryScript(new ImaginaryConstructableObject()), "Test script");
					}
				}
			}
			ImGui.End();
		}

		private static void ModifyScript(string scriptName, ImaginaryScript script)
		{
			ImGui.TextDisabled(scriptName);
			ImGui.Text("Cannot yet be changed from here.");

			if (ImGui.Button("Remove"))
			{
				MainClass.RemoveScript(currentSelectedHierarchyObject, scriptName);
			}
		}

		static bool enableHierarchyViewer = true;
		private static void HierarchyViewer(ImaginaryHierarchyObject rootHierarchyObject)
		{
			if (!enableHierarchyViewer)
			{
				return;
			}

			ImGui.Begin("Hierarchy Viewer");
			{
				CreateTreeForHierarchyObject(rootHierarchyObject);

				if (ImGui.Button("Add HierarchyObject"))
				{
					NewHierarchyObject();
				}
			}
			ImGui.End();
		}

		private static void NewHierarchyObject()
		{
			MainClass.AddHierarchyObject(currentSelectedHierarchyObject, new ImaginaryHierarchyObject(currentSelectedHierarchyObject, new ImaginaryConstructableObject(typeof(HierarchyObject))), "Test");
		}

		private static void CreateTreeForHierarchyObject(ImaginaryHierarchyObject hierarchyObject)
		{
			if (ImGui.TreeNodeEx(hierarchyObject.Name, currentSelectedHierarchyObject == hierarchyObject ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None))
			{
				ImGui.SameLine();

				if (ImGui.Button("Select"))
				{
					currentSelectedHierarchyObject = hierarchyObject;
				}

				foreach (var hierarchyObjectChild in hierarchyObject.LocalHierarchy)
				{
					CreateTreeForHierarchyObject(hierarchyObject.LocalHierarchy[hierarchyObjectChild.Key]);
				}
				ImGui.TreePop();
			}
		}
	}
}
