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
		public static ImaginaryHierarchyObject RootHierarchyObject;
		public static ImaginaryHierarchyObject CurrentSelectedHierarchyObject;
		public static Assembly UserAssembly;

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

					MainClass.ParseCommand(line, ref RootHierarchyObject, ref CurrentSelectedHierarchyObject, UserAssembly);
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

				UI(ref RootHierarchyObject, ref CurrentSelectedHierarchyObject, UserAssembly);

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
			UserAssembly = MainClass.Compile();

			MainClass.Analyze(UserAssembly);

			var fileSystemWatcher = new FileSystemWatcher(CurrentProject.ScriptsDirectory.FullName, "*.cs");
			fileSystemWatcher.Changed += (_, _1) =>
			{
				Output.Log("Code change detected, recompiling.");
				CodeFilePaths = Directory.GetFiles(CurrentProject.ScriptsDirectory.FullName, "*.cs");
				// TODO: something to wait until the file is ready.
				Thread.Sleep(100);
				MainClass.Compile();
				MainClass.Analyze(UserAssembly);
			};
			fileSystemWatcher.EnableRaisingEvents = true;

			RootHierarchyObject =
				new ImaginaryHierarchyObject(null, new ImaginaryConstructableObject(typeof(HierarchyRoot)));

			Output.OutputLogged += (string newLog) => consoleLog += newLog + Environment.NewLine;
		}
	}
}
