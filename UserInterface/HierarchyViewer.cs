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