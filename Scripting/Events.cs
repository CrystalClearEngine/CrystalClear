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
		public delegate void StartEventHandler(object o, EventArgs args);
		public static class StartEventClass
		{
			public static void RaiseStartEvent()
			{
				StartEvent?.Invoke(null, new EventArgs(/*any info you want handlers to have*/));
			}

			public static event StartEventHandler StartEvent;
		}

		public delegate void ExitEventHandler(object o, EventArgs args);
		public static class ExitEventClass
		{
			public static void RaiseExitEvent()
			{
				StartEvent?.Invoke(null, new EventArgs(/*any info you want handlers to have*/));
			}

			public static event ExitEventHandler StartEvent;
		}
	}
}