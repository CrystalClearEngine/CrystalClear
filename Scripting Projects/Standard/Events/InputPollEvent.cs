using CrystalClear.EventSystem;
using System;
using System.Diagnostics;
using System.Threading;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The frame update event attribute.
	/// </summary>
	public class OnInputPollAttribute : SubscribeToAttribute
	{
		public OnInputPollAttribute() : base(typeof(FrameUpdateEventClass))
		{
		}
	}

	/// <summary>
	/// The frame update event class.
	/// </summary>
	public class InputPollEvent : SingletonScriptEventHandlerScriptEvent<InputPollEvent>
	{
		public static void InputPollLoop(int PollFreqInMS = 0)
		{
			Stopwatch stopwatch = new Stopwatch(); // TODO remember to remove these or disable when not necessary or requested!
			while (true)
			{
				stopwatch.Start();
				if (PollFreqInMS > 0) Thread.Sleep(PollFreqInMS); // TODO replace Thread.Sleep with more reliable waiting system.
				Instance.RaiseEvent();
				Console.WriteLine(Math.Round(1 / stopwatch.Elapsed.TotalSeconds) + " PPS");
				stopwatch.Stop(); stopwatch.Reset();
			}
		}
	}
}