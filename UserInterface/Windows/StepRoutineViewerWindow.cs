using CrystalClear.HierarchySystem;
using CrystalClear.ScriptUtilities.StepRoutines;
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
	public class StepRoutineViewerWindow : EditorWindow
	{
		public override string WindowTitle { get; set; } = "StepRoutine Viewer";

		protected override void UIImpl()
		{
			StepRoutineViewerUI();
		}

		public void StepRoutineViewerUI()
		{
			foreach (var stepRoutine in StepRoutineManager.EnumerateStepRoutines())
			{
				DisplayStepRoutine(stepRoutine);
			}
		}

		private void DisplayStepRoutine(StepRoutineInfo stepRoutineInfo)
		{
			ImGui.Text($"Name: {stepRoutineInfo.StepRoutineName ?? "null"}");
			ImGui.Text($"ID: {stepRoutineInfo.StepRoutineId}");
			ImGui.Text($"State: {Enum.GetName(typeof(StepRoutineState), stepRoutineInfo.State)}");
			if (stepRoutineInfo.CurrentWaitFor is null)
			{
				ImGui.Text("Not waiting for anything.");
			}
			else
			{
				ImGui.Text($"Wait type: {stepRoutineInfo.CurrentWaitFor.GetType().Name}");
				ImGui.Text($"Additional wait info: {stepRoutineInfo.CurrentWaitFor.ToString() ?? "null"}");
			}
		}
	}
}