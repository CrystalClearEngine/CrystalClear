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
	}
}