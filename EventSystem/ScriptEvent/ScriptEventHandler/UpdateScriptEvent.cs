using System;
using System.Diagnostics;
using System.Threading;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// A singleton version of the EventArgsScriptEvent. Contains implementation for a singleton.
	/// </summary>
	/// <typeparam name="TInstance">The type of the instance. Should generally be the same as the deriving class.</typeparam>
	public abstract class UpdateScriptEvent<TInstance>
		: SingletonScriptEventHandlerScriptEvent<TInstance>
		where TInstance : UpdateScriptEvent<TInstance>, new()
	{
		// Singleton stuff.

		// Static constructor to ensure that SingletonScriptEvent can't be instanciated.
		static UpdateScriptEvent()
		{
		}

		// Protected parameterless constructor for new T() and deriving classes.
		protected UpdateScriptEvent()
		{
		}

		~UpdateScriptEvent()
		{
			timer?.Dispose();
		}

		// TODO: decide if these should be static.

		private Stopwatch stopwatch = new Stopwatch();

		private Timer timer;

		private Thread thread;

		public static double DeltaTimeInSeconds { get => Instance.stopwatch.Elapsed.TotalSeconds; }

		public static TimeSpan DeltaTime { get => Instance.stopwatch.Elapsed; }

		// TODO: add check so it can't Raise the event if it is already being raised from last time? Maybe an IsStillBeingRaised field in Instance?
		public static void Start(TimeSpan interval = new TimeSpan())
		{
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
				// TODO: determine wether to use ThreadPool for this. (Maybe only for select events, these ones probably need precicion more...)
				Instance.thread = new Thread(UpdateLoop);
				Instance.thread.Start();
			}
		}

		public static void Stop()
		{
			Instance.timer?.Dispose();
			Instance.thread?.Abort();
		}

		private static void UpdateLoop()
		{
			while (true)
			{
				Instance.RaiseEvent();
				Instance.stopwatch.Restart();
			}
		}
	}
}
