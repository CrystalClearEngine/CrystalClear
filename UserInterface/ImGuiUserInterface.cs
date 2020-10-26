using ImGuiNET;
using System.Diagnostics;
using System.Threading;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace CrystalClear.UserInterface
{
	public static class UserInterface
	{
		public static void Main()
		{
			Sdl2Window window = WindowingSystem.WindowingSystem.CreateNewWindow("Veldrid test", width: 500, height: 500);

			GraphicsDeviceOptions gdOptions = new GraphicsDeviceOptions(false,
															   null,
															   false,
															   ResourceBindingModel.Improved,
															   true,
															   true,
															   true);

			var graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, gdOptions);

			ImGuiRenderer imguiRenderer = new ImGuiRenderer(
				graphicsDevice, graphicsDevice.MainSwapchain.Framebuffer.OutputDescription,
				(int)graphicsDevice.MainSwapchain.Framebuffer.Width, (int)graphicsDevice.MainSwapchain.Framebuffer.Height);

			var cl = graphicsDevice.ResourceFactory.CreateCommandList();

			window.Resized += () => imguiRenderer.WindowResized(window.Width, window.Height);
			window.Resized += () => graphicsDevice.ResizeMainWindow((uint)window.Width, (uint)window.Height);

			ImGui.StyleColorsClassic();

			int optimalFrameTimeMS = 16;
			bool showMore = false;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (window.Exists)
			{
				Thread.Sleep(optimalFrameTimeMS);

				var input = window.PumpEvents();
				if (!window.Exists) { break; }
				imguiRenderer.Update((float)stopwatch.Elapsed.TotalSeconds, input); // Compute actual value for deltaSeconds.

				// Draw stuff
				ImGui.Begin("Hierarchy");
				{

				}
				ImGui.End();

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
	}
}
