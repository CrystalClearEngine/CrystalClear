using System;

namespace CrystalClear.Scripting.EventSystem.Events
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
			StartEventDelegate?.Invoke();
		}

		public static StartEventHandler StartEventDelegate;
	}
}