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

		protected SubscribeToAttribute()
		{

		}
	}

	namespace Events
	{
		public class OnStartEvent : SubscribeToAttribute
		{
			public OnStartEvent()
			{
				EventType = typeof(StartEventHandler);
			}
		}
		public delegate void StartEventHandler(EventArgs args);
		public static class StartEventClass
		{
			public static void RaiseStartEvent()
			{
				StartEvent?.Invoke(new EventArgs(/*any info you want handlers to have*/));
			}

			public static event StartEventHandler StartEvent;
		}

		public class OnExitEvent : SubscribeToAttribute
		{
			public OnExitEvent()
			{
				EventType = typeof(StartEventHandler);
			}
		}
		public delegate void ExitEventHandler(EventArgs args);
		public static class ExitEventClass
		{
			public static void RaiseExitEvent()
			{
				ExitEvent?.Invoke(new EventArgs(/*any info you want handlers to have*/));
			}

			public static event ExitEventHandler ExitEvent;
		}
	}
}