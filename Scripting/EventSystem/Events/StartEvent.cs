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
			StartEvent?.Invoke();
		}

		public static event StartEventHandler StartEvent;
	}
}