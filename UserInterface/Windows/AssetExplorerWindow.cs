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
	// TODO: cache the files and folders and only periodically update to avoid taxing the hardware?
	public class AssetExplorerWindow : EditorWindow
	{
		public override string WindowTitle { get; set; } = "Asset Explorer";

		protected override void UIImpl()
		{
			AssetExplorerUI();
		}

		public void AssetExplorerUI()
		{
			if (ImGui.Button("Assets"))
			{
				history.Clear();
				history.Push(CurrentProject.AssetsPath);
			}

			ImGui.SameLine();

			if (ImGui.Button("Hierarchies"))
			{
				history.Clear();
				history.Push(CurrentProject.HierarchyPath);
			}

			ImGui.SameLine();

			if (ImGui.Button("Scripts"))
			{
				history.Clear();
				history.Push(CurrentProject.ScriptsPath);
			}

			ImGui.SameLine();

			if (ImGui.Button("Builds"))
			{
				history.Clear();
				history.Push(CurrentProject.BuildPath);
			}

			Explore();
		}

		Stack<string> history = new Stack<string>(new string[] { CurrentProject.AssetsPath });

		private void Explore()
		{
			ImGui.Text("Contents in " + history.Peek());

			if (history.Count > 1 && ImGui.Button("<"))
			{
				history.Pop();
			}

			if (ImGui.ListBoxHeader(""))
			{
				foreach (var directoryPath in Directory.GetDirectories(history.Peek()))
				{
					DisplayDirectory(directoryPath);
				}

				foreach (var filePath in Directory.GetFiles(history.Peek()))
				{
					DisplayFile(filePath);
				}

				ImGui.ListBoxFooter();
			}
		}

		private void DisplayDirectory(string directoryPath)
		{
			if (ImGui.Button(Path.GetFileName(directoryPath)))
			{
				history.Push(directoryPath);
			}
		}

		private void DisplayFile(string filePath)
		{
			if (ImGui.Button(Path.GetFileName(filePath)))
			{
				OpenWithDefaultProgram(filePath);
			}
		}

		private void OpenWithDefaultProgram(string path)
		{
			// TODO: make work on other operating systems than windows.

			Process fileopener = new Process();
			fileopener.StartInfo.FileName = "explorer";
			fileopener.StartInfo.Arguments = "\"" + path + "\"";
			fileopener.Start();
		}
	}
}