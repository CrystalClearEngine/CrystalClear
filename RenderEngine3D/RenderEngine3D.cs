using System;
using System.Collections.Generic;
using System.Text;
using CrystalClear.WindowingSystem;
using Veldrid.Sdl2;
using Veldrid;
using Veldrid.StartupUtilities;
using ImGuiNET;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Veldrid.ImageSharp;
using Veldrid.Utilities;
using System.Runtime.CompilerServices;

namespace RenderEngine3D
{
	public static class RenderEngine3D
	{
		public static void Main()
		{
			Sdl2Window window = WindowingSystem.CreateNewWindow("Veldrid test");

			GraphicsDeviceOptions gdOptions = new GraphicsDeviceOptions(false,
															   null,
															   false,
															   ResourceBindingModel.Improved,
															   true,
															   true,
															   true);

			var graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, gdOptions, GraphicsBackend.Direct3D11);

			var imGuiRenderer = new ImGuiRenderer(graphicsDevice, new OutputDescription(), window.Width, window.Height);

			CommandList commandList = graphicsDevice.ResourceFactory.CreateCommandList();

			ImGui.StyleColorsClassic();

			while (window.Exists)
			{
				commandList.Begin();
				InputSnapshot inputSnapshot = window.PumpEvents();
				Sdl2Events.ProcessEvents();

				imGuiRenderer.CreateDeviceResources(graphicsDevice, graphicsDevice.SwapchainFramebuffer.OutputDescription, ColorSpaceHandling.Linear);

				ImGui.Begin("Test");
				ImGui.Button("Hello");
				ImGui.End();

				imGuiRenderer.Update(1, inputSnapshot);
				imGuiRenderer.Render(graphicsDevice, commandList);
				ImGui.Render();
				commandList.End();

				graphicsDevice.SubmitCommands(commandList);

				graphicsDevice.WaitForIdle();

				graphicsDevice.SwapBuffers();
			}
		}
    }
}
