using System;
using System.Collections.Generic;
using System.Text;
using CrystalClear.WindowingSystem;
using SFML.Window;
using Veldrid;

namespace RenderEngine3D
{
	public static class RenderEngine3D
	{
		public static void Main()
		{
			Window window = WindowingSystem.CreateNewWindow("Veldrid test", VideoMode.DesktopMode);

			GraphicsDeviceOptions gdOptions = new GraphicsDeviceOptions(false,
															   null,
															   false,
															   ResourceBindingModel.Improved,
															   true,
															   true,
															   true);
		}
	}
}
