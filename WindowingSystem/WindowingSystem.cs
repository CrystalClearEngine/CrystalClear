using System;
using System.Collections.Generic;
using SFML.Window;

namespace CrystalClear.WindowingSystem
{
	// TODO: create OnWindowEventDispatch event?
	public static class WindowingSystem
	{
		public static Window MainWindow { get; }

		private static List<Window> windows;
		public static IReadOnlyCollection<Window> Windows => windows.AsReadOnly();
	}
}
