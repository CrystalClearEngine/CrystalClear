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
	public static class UserInterface
	{
		private static ImaginaryHierarchyObject rootHierarchyObject;
		private static ImaginaryHierarchyObject currentSelectedHierarchyObject;
		private static Assembly userAssembly;

		public static void Main()
		{
			Sdl2Window window;
			GraphicsDevice graphicsDevice;
			ImGuiRenderer imguiRenderer;
			CommandList cl;

			SetUpRenderer(out window, out graphicsDevice, out imguiRenderer, out cl);

			window.Resized += () => imguiRenderer.WindowResized(window.Width, window.Height);
			window.Resized += () => graphicsDevice.ResizeMainWindow((uint)window.Width, (uint)window.Height);

			ChooseProject();

			SetUpEditor();

			// Allow using the console interface simultaneously. Might cause some issues if commands are issued simultaneously from the GUI and CLI, but that probably won't happen often.
			Task.Run(() =>
			{
				while (true)
				{
					Output.Log();

					var line = Console.ReadLine();

					MainClass.ParseCommand(line, ref rootHierarchyObject, ref currentSelectedHierarchyObject, userAssembly);
				}
			});

			int optimalFrameTimeMS = 16;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (window.Exists)
			{
				Thread.Sleep((int)(optimalFrameTimeMS - stopwatch.ElapsedMilliseconds));

				var input = window.PumpEvents();
				if (!window.Exists) { break; }
				imguiRenderer.Update((float)stopwatch.Elapsed.TotalSeconds, input); // Compute actual value for deltaSeconds.

				UI(ref rootHierarchyObject, ref currentSelectedHierarchyObject, userAssembly);

				cl.Begin();
				cl.SetFramebuffer(graphicsDevice.MainSwapchain.Framebuffer);
				cl.ClearColorTarget(0, RgbaFloat.Black);
				imguiRenderer.Render(graphicsDevice, cl);
				cl.End();
				graphicsDevice.SubmitCommands(cl);
				graphicsDevice.SwapBuffers(graphicsDevice.MainSwapchain);

				stopwatch.Restart();
			}
		}

		private static void ChooseProject()
		{
			Output.Log("Please open or create a new project:");
			ProjectSelection:
			switch (Console.ReadLine())
			{
				case "new":
					ProjectInfo.NewProject(AskQuestion("Pick a path for the new project"),
						AskQuestion("Pick a name for the new project"));
					break;

				case "open":
					try
					{
						ProjectInfo.OpenProject(AskQuestion("Enter the path of the project"));
					}
					catch (ArgumentException)
					{
						goto ProjectSelection;
					}

					break;

				default:
					Output.ErrorLog("command error: unknown command");
					goto ProjectSelection;
			}
		}

		private static void SetUpRenderer(out Sdl2Window window, out GraphicsDevice graphicsDevice, out ImGuiRenderer imguiRenderer, out CommandList cl)
		{
			window = WindowingSystem.WindowingSystem.CreateNewWindow($"Crystal Clear Engine {CrystalClearVersion}", width: 500, height: 500);
			GraphicsDeviceOptions gdOptions = new GraphicsDeviceOptions(false,
															   null,
															   false,
															   ResourceBindingModel.Improved,
															   true,
															   true,
															   true);

			graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, gdOptions);
			imguiRenderer = new ImGuiRenderer(
				graphicsDevice, graphicsDevice.MainSwapchain.Framebuffer.OutputDescription,
				(int)graphicsDevice.MainSwapchain.Framebuffer.Width, (int)graphicsDevice.MainSwapchain.Framebuffer.Height);
			cl = graphicsDevice.ResourceFactory.CreateCommandList();

			ImGui.StyleColorsClassic();
		}

		private static void SetUpEditor()
		{
			{
				FileInfo[] files = CurrentProject.ScriptsDirectory.GetFiles("*.cs");

				CodeFilePaths = new string[files.Length];

				for (var i = 0; i < files.Length; i++)
					CodeFilePaths[i] = files[i].FullName;
			}

			// Compile the code.
			userAssembly = MainClass.Compile();

			MainClass.Analyze(userAssembly);

			var fileSystemWatcher = new FileSystemWatcher(CurrentProject.ScriptsDirectory.FullName, "*.cs");
			fileSystemWatcher.Changed += (_, _1) =>
			{
				Output.Log("Code change detected, recompiling.");
				CodeFilePaths = Directory.GetFiles(CurrentProject.ScriptsDirectory.FullName, "*.cs");
				// TODO: something to wait until the file is ready.
				Thread.Sleep(100);
				MainClass.Compile();
				MainClass.Analyze(userAssembly);
			};
			fileSystemWatcher.EnableRaisingEvents = true;

			rootHierarchyObject =
				new ImaginaryHierarchyObject(null, new ImaginaryConstructableObject(typeof(HierarchyRoot)));

			rootHierarchyObject.LocalHierarchy.Add("Child1", new ImaginaryHierarchyObject(rootHierarchyObject, new ImaginaryConstructableObject(typeof(HierarchyObject))));
			rootHierarchyObject.LocalHierarchy.Add("Child2", new ImaginaryHierarchyObject(rootHierarchyObject, new ImaginaryConstructableObject(typeof(HierarchyObject))));
			rootHierarchyObject.LocalHierarchy.Add("Child3", new ImaginaryHierarchyObject(rootHierarchyObject, new ImaginaryConstructableObject(typeof(HierarchyObject))));

			rootHierarchyObject.LocalHierarchy["Child1"].LocalHierarchy.Add("ChildChild1", new ImaginaryHierarchyObject(rootHierarchyObject.LocalHierarchy["Child1"], new ImaginaryConstructableObject(typeof(HierarchyObject))));

			Output.OutputLogged += (string newLog) => consoleLog += newLog + Environment.NewLine;
		}

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
					if(ImGui.MenuItem("Save Project"))
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
			} ImGui.EndMainMenuBar();
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
					MainClass.ParseCommand(consoleInput, ref rootHierarchyObject, ref currentSelectedHierarchyObject, userAssembly);

					Output.Log(consoleInput);

					consoleInput = string.Empty;
				}
			} ImGui.End();
		}

		static bool enableModifier = true;
		// TODO: make general purpose.
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

					if(ImGui.Button("Attatch Script"))
					{
						MainClass.AddScript(currentSelectedHierarchyObject, new ImaginaryScript(new ImaginaryConstructableObject()), "Test script");
					}
				}
			} ImGui.End();
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
			} ImGui.End();
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
