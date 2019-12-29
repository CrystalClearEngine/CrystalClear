using CrystalClear.EventSystem;
using System;
using System.Diagnostics;
using System.Threading;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The frame update event attribute.
	/// </summary>
	public class OnFrameUpdateAttribute : SubscribeToAttribute
	{
		public OnFrameUpdateAttribute() : base(typeof(FrameUpdateEventClass))
		{
		}
	}

	/// <summary>
	/// The frame update event class.
	/// </summary>
	public class FrameUpdateEventClass /*TODO Remove "Class" suffix!*/ : SingletonScriptEventHandlerScriptEvent<FrameUpdateEventClass>
	{
		public static void FrameUpdateLoop()
		{
			Stopwatch stopwatch = new Stopwatch();
			while (true)
			{
				stopwatch.Start();
				//Thread.Sleep(0); // Artificial slowdown or FPS limiter! Alternative: Thread.Sleep(1000/<FPS>);
				Instance.RaiseEvent();
				Console.WriteLine(Math.Round(1 / stopwatch.Elapsed.TotalSeconds) + " FPS");
				stopwatch.Stop(); stopwatch.Reset();
			}
		}
	}
}