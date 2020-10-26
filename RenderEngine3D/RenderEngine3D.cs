using CrystalClear.WindowingSystem;
using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace RenderEngine3D
{
	public static class RenderEngine3D
	{
		public static void Main()
		{
			Sdl2Window window = WindowingSystem.CreateNewWindow("Veldrid test", width: 1920, height: 1080);

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

			while (window.Exists)
			{
				var input = window.PumpEvents();
				if (!window.Exists) { break; }
				imguiRenderer.Update(1f / 60f, input); // Compute actual value for deltaSeconds.

				// Draw stuff
				ImGui.Text("Hello World");

				cl.Begin();
				cl.SetFramebuffer(graphicsDevice.MainSwapchain.Framebuffer);
				cl.ClearColorTarget(0, RgbaFloat.Black);
				imguiRenderer.Render(graphicsDevice, cl);
				cl.End();
				graphicsDevice.SubmitCommands(cl);
				graphicsDevice.SwapBuffers(graphicsDevice.MainSwapchain);
			}
		}
	}
}
