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
	// TODO: cache the files and folders and only periodically update to avoid taxing the hardware?
	public static partial class UserInterface
	{
		static bool enableAssetExplorer = true;
		public static void AssetExplorer()
		{
			if (!enableAssetExplorer)
			{
				return;
			}

			ImGui.Begin("Asset Explorer");
			{
				ImGui.BeginTabBar("AssetExplorerTabBar");
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
				} ImGui.EndTabBar();

				Explore();
			} ImGui.End();
		}

		static Stack<string> history = new Stack<string>(new string[] { CurrentProject.AssetsPath });
		private static void Explore()
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

		private static void DisplayDirectory(string directoryPath)
		{
			if (ImGui.Button(Path.GetFileName(directoryPath)))
			{
				history.Push(directoryPath);
			}
		}

		private static void DisplayFile(string filePath)
		{
			if (ImGui.Button(Path.GetFileName(filePath)))
			{
				OpenWithDefaultProgram(filePath);
			}
		}

		private static void OpenWithDefaultProgram(string path)
		{
			// TODO: make work on other operating systems than windows.

			Process fileopener = new Process();
			fileopener.StartInfo.FileName = "explorer";
			fileopener.StartInfo.Arguments = "\"" + path + "\"";
			fileopener.Start();
		}
	}
}