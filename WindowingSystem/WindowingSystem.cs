using System;
using System.Collections.Generic;
using SFML.Window;

namespace CrystalClear.WindowingSystem
{
	// TODO: create OnWindowEventDispatch event?
	public static class WindowingSystem
	{
		// TODO: is this necessary?
		public static Window MainWindow => Windows[0];

		private static List<Window> windows;
		
		// TODO: is readonly necessary?
		public static IReadOnlyList<Window> Windows => windows.AsReadOnly();
	}
}
