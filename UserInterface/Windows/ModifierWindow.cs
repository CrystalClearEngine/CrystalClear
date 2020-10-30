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
	// TODO: make general purpose.
	public class ModifierWindow : EditorWindow
	{
		public override string WindowTitle { get; protected set; } = "Modifier";

		protected override void UIImpl()
		{
			ModifierUI();
		}

		public void ModifierUI()
		{
			if (CurrentSelectedHierarchyObject is null)
			{
				ImGui.Text("Nothing selected.");
				return;
			}

			string name = CurrentSelectedHierarchyObject.Name;
			ImGui.InputText("Name", ref name, 100);
			CurrentSelectedHierarchyObject.Name = name;

			if (ImGui.Button("Delete"))
			{
				MainClass.DeleteHierarchyObject(CurrentSelectedHierarchyObject.Parent, CurrentSelectedHierarchyObject.Name);
				CurrentSelectedHierarchyObject = null;
				return;
			}

			if (ImGui.CollapsingHeader("Attatched Scripts"))
			{
				foreach (var script in CurrentSelectedHierarchyObject.AttachedScripts)
				{
					ModifyScript(script.Key, script.Value);
				}

				if (ImGui.Button("Attatch Script"))
				{
					MainClass.AddScript(CurrentSelectedHierarchyObject, new ImaginaryScript(new ImaginaryConstructableObject()), "Test script");
				}
			}
		}

		private void ModifyScript(string scriptName, ImaginaryScript script)
		{
			ImGui.TextDisabled(scriptName);
			ImGui.Text("Cannot yet be changed from here.");

			if (ImGui.Button("Remove"))
			{
				MainClass.RemoveScript(CurrentSelectedHierarchyObject, scriptName);
			}
		}
	}
}