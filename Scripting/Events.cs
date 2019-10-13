using System;

namespace CrystalClear.Scripting.EventSystem
{
	public class SubscribeToAttribute : Attribute
	{
		public Type EventType;

		public SubscribeToAttribute(Type @event)
		{
			EventType = @event;
		}

		protected SubscribeToAttribute() { }
	}

	public class CancellableEvent : EventArgs
	{
		public bool IsCancelled { get; protected set; }

		public void Cancel()
		{
			IsCancelled = true;
		}
	}

	namespace Events
	{
		public class OnStartEventAttribute : SubscribeToAttribute
		{
			public OnStartEventAttribute()
			{
				EventType = typeof(StartEventHandler);
			}
		}
		public delegate void StartEventHandler();
		public static class StartEventClass
		{
			public static void RaiseStartEvent()
			{
				StartEvent?.Invoke();
			}

			public static event StartEventHandler StartEvent;
		}


		public class OnExitEventAttribute : SubscribeToAttribute
		{
			public OnExitEventAttribute()
			{
				EventType = typeof(ExitEventHandler);
			}
		}


		public class ExitEventArgs : CancellableEvent { }
		public delegate void ExitEventHandler(ExitEventArgs args);
		public static class ExitEventClass
		{
			public static ExitEventArgs RaiseExitEvent()
			{
				ExitEventArgs exitEventArgs = new ExitEventArgs();
				ExitEvent?.Invoke(exitEventArgs);
				return exitEventArgs;
			}

			public static event ExitEventHandler ExitEvent;
		}
	}
}