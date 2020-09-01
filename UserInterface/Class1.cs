using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using TGUI;
using System.Diagnostics;
using CrystalClear;
using System.Threading;

namespace UserInterface
{
	public static class TestInterface
	{
		public static void Main()
		{
			RenderWindow window = new RenderWindow(VideoMode.FullscreenModes[1], "Hello, TGUI and SFML.")
			{
				Size = new Vector2u(1920, 1080)
			};
			window.SetFramerateLimit(100);

			window.Closed += (s,e) => window.Close();

			Gui gui = new Gui(window);

			gui.LoadWidgetsFromFile(@".\Forms\CrystalClearEngineDesign.txt");

			ProgressBar progressBar = (ProgressBar)gui.Get("ProgressBar");

			ChatBox consoleOut = (ChatBox)gui.Get("ConsoleOut");

			Output.OutputLogged += consoleOut.AddLine;

			EditBox editBox = (EditBox)gui.Get("ConsoleInput");

			Button consoleSend = (Button)gui.Get("ConsoleSend");

			consoleSend.Clicked += (s,e) =>
			{
				Output.Log(editBox.Text);
				editBox.Text = string.Empty;
			};

			Tabs playControls = (Tabs)gui.Get("PlayControls");

			playControls.TabSelected += (s, e) =>
			{
				switch (e.Value)
				{
					case ">":
						break;
				}
			};

			ChildWindow projectManager = (ChildWindow)gui.Get("ProjectManager");

			EditBox answer = (EditBox)gui.Get("Answer");
			Label request = (Label)gui.Get("Request");

			Button openProject = (Button)gui.Get("OpenProject");

			Button newProject = (Button)gui.Get("NewProject");

			Button confirmNewProject = (Button)gui.Get("ConfirmNewProject");

			openProject.Clicked += (s,e) =>
			{
				newProject.Enabled = false;

				ProjectInfo.OpenProject(answer.Text);

				window.SetTitle($"{EditorInformation.CurrentProject.ProjectName} | Crystal Clear Engine {CrystalClearInformation.CrystalClearVersion}");

				gui.Remove(projectManager);
			};

			newProject.Clicked += (s, e) =>
			{
				string projectPath = answer.Text;

				openProject.Enabled = false;

				newProject.Enabled = false;

				newProject.Visible = false;

				request.Text = "Project name:";

				answer.Text = string.Empty;

				confirmNewProject.Enabled = true;

				confirmNewProject.Visible = true;

				confirmNewProject.Clicked += (s,e) =>
				{
					ProjectInfo.NewProject(projectPath, answer.Text);

					window.SetTitle($"{EditorInformation.CurrentProject.ProjectName} | Crystal Clear Engine {CrystalClearInformation.CrystalClearVersion}");

					gui.Remove(projectManager);
				};
			};

			TreeView hierarchyView = (TreeView)gui.Get("HierarchyViewer");

			Button newHieararchyObjectButton = (Button)gui.Get("NewHierarchyObjectButton");

			newHieararchyObjectButton.Clicked += (s, e) =>
			{
				gui.LoadWidgetsFromFile(@".\Forms\NewHierarchyObjectDesign.txt", false);
			};

			Label framerateValue = (Label)gui.Get("FramerateValue");
			Label frametimeValue = (Label)gui.Get("FrametimeValue");

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (window.IsOpen)
			{
				window.DispatchEvents();

				UpdateWindow(window, gui);

				framerateValue.Text = Math.Round(1 / stopwatch.Elapsed.TotalSeconds) + " FPS";
				frametimeValue.Text = stopwatch.Elapsed.Milliseconds + " ms";

				stopwatch.Restart();
			}
		}

		static void UpdateWindow(RenderWindow window, Gui gui)
		{
			window.Clear(new Color(177, 177, 177));
			gui.Draw();
			window.Display();
		}
	}
}
