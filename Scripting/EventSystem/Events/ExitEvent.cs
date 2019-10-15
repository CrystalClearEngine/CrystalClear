using System;

namespace CrystalClear.Scripting.EventSystem.Events
{
	public class OnExitEventAttribute : SubscribeToAttribute
	{
		public OnExitEventAttribute()
		{
			EventType = typeof(ExitEventHandler);
		}
	}

	public class ExitEventArgs : CancellableEventArgs
	{
	}

	public delegate void ExitEventHandler(ExitEventArgs args);

	public static class ExitEventClass
	{
		public static ExitEventArgs RaiseExitEvent()
		{
			ExitEventArgs exitEventArgs = new ExitEventArgs();
			ExitEventDelegate?.Invoke(exitEventArgs);
			return exitEventArgs;
		}

		public static ExitEventHandler ExitEventDelegate;
	}
}
