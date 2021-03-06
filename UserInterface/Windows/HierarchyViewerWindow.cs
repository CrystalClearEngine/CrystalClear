﻿using CrystalClear.HierarchySystem;
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
	public class HierarchyViewerWindow : EditorWindow
	{
		public override string WindowTitle { get; set; } = "Hierarchy Viewer";

		protected override void UIImpl()
		{
			HierarchyViewerUI();
		}

		public void HierarchyViewerUI()
		{
			CreateTreeForHierarchyObject(RootHierarchyObject);

			if (ImGui.Button("Add HierarchyObject"))
			{
				NewHierarchyObject();
			}
		}

		private void NewHierarchyObject()
		{
			MainClass.AddHierarchyObject(CurrentSelectedHierarchyObject, new ImaginaryHierarchyObject(CurrentSelectedHierarchyObject, new ImaginaryConstructableObject(typeof(HierarchyObject))), "Test");
		}

		private void CreateTreeForHierarchyObject(ImaginaryHierarchyObject hierarchyObject)
		{
			if (ImGui.TreeNodeEx(hierarchyObject.Name, ReferenceEquals(CurrentSelectedHierarchyObject, hierarchyObject) ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) || ReferenceEquals(CurrentSelectedHierarchyObject, hierarchyObject))
			{
				ImGui.SameLine();

				if (ImGui.Button("Select"))
				{
					CurrentSelectedHierarchyObject = hierarchyObject;
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