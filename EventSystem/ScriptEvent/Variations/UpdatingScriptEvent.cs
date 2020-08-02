using System;
using System.Diagnostics;
using System.Threading;

namespace CrystalClear.EventSystem
{
	public abstract class UpdatingScriptEvent<TInstance>
		: ScriptEvent<TInstance>
		where TInstance : UpdatingScriptEvent<TInstance>, new()
	{
		~UpdatingScriptEvent()
		{
			timer?.Dispose();
		}

		// TODO: decide if these should be static.

		private Stopwatch stopwatch = new Stopwatch();

		private Timer timer;

		private Thread thread;

		private bool running = false;

		public static double DeltaTimeInSeconds { get => Instance.stopwatch.Elapsed.TotalSeconds; }

		public static TimeSpan DeltaTime { get => Instance.stopwatch.Elapsed; }

		// TODO: add check so it can't Raise the event if it is already being raised from last time? Maybe an IsStillBeingRaised field in Instance?
		public static void Start(TimeSpan interval = new TimeSpan())
		{
			Instance.running = true;

			// TODO: attempt to raise the Event on the Main thread, to prevent multi threading issues (or atleast as an option, or maybe variants of the events etc... they could easily be determined using the same attribute as now, only with constructors)
			if (interval != TimeSpan.Zero)
			{
				// TODO: determine if a timer is suitable for this at all.
				Instance.timer = new Timer((object obj)
					=>
					{
						Instance.RaiseEvent();
						Instance.stopwatch.Restart();
					}
					, null, TimeSpan.Zero, interval);
			}
			else
			{
				// TODO: determine whether to use ThreadPool for this. (Maybe only for select events, these ones probably need precicion more...)
				Instance.thread = new Thread(UpdateLoop);
				Instance.thread.Start();
			}
		}

		public static void Stop()
		{
			if (!Instance.running)
				return;

			Instance.running = false;
			Instance.timer?.Dispose();
			//Instance.thread?.Abort();
		}

		private static void UpdateLoop()
		{
			while (Instance.running)
			{
				Instance.RaiseEvent();
				Instance.stopwatch.Restart();
			}
		}
	}
}
