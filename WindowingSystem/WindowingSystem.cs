using System;
using System.Collections.Generic;
using System.Threading;
using SFML.System;
using SFML.Window;

namespace CrystalClear.WindowingSystem
{
	// TODO: create OnWindowEventDispatch event?
	public static class WindowingSystem
	{
		public static Window CreateNewWindow(string title, VideoMode videoMode, Vector2u? size = null, Vector2i? position = null)
		{
			var window = new Window(videoMode, title);
			if (size.HasValue)
			{
				window.Size = size.Value;
			}
			if (position.HasValue)
			{
				window.Position = position.Value;
			}
			
			windows.Add(window);

			return window;
		}
		
		// TODO: is this necessary?
		public static Window MainWindow => Windows[0];

		private static List<Window> windows = new List<Window>();
		
		// TODO: is readonly necessary?
		public static IReadOnlyList<Window> Windows => windows.AsReadOnly();
	}
}
