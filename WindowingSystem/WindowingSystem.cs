using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Veldrid.Sdl2;

namespace CrystalClear.WindowingSystem
{
	// TODO: create OnWindowEventDispatch event?
	public static class WindowingSystem
	{
		public static Sdl2Window CreateNewWindow(string title, int xPosition = 50, int yPosition = 50, int width = 600, int height = 400, SDL_WindowFlags flags = SDL_WindowFlags.Resizable | SDL_WindowFlags.Shown, bool threadedProcessing = false)
		{
			var window = new Sdl2Window(title, xPosition, yPosition, width, height, flags, threadedProcessing);
			
			windows.Add(window);

			return window;
		}
		
		// TODO: is this necessary?
		public static Sdl2Window MainWindow => Windows[0];

		private static List<Sdl2Window> windows = new List<Sdl2Window>();
		
		// TODO: is readonly necessary?
		public static IReadOnlyList<Sdl2Window> Windows => windows.AsReadOnly();
	}
}
