using System;
using System.Reflection;

namespace CrystalClear.EventSystem.Events
{
	public class OnStartEventAttribute : SubscribeToAttribute
	{
		public OnStartEventAttribute()
		{
			EventType = typeof(StartEventClass.StartEventHandler);
		}
	}

	public class StartEventClass : IEventClass
	{
		public delegate void StartEventHandler();

		public StartEventHandler StartEventDelegate;

		public void Subscribe(Delegate eventHandler)
		{
			StartEventDelegate += (StartEventHandler) eventHandler;
		}

		public void Subscribe(MethodInfo method, object instance)
		{
			Delegate eventHandler = Delegate.CreateDelegate(typeof(StartEventHandler), instance, method);
			Subscribe(eventHandler);
		}

		public void UnSubscribe(Delegate eventHandler)
		{
			Delegate.Remove(StartEventDelegate, eventHandler);
		}

		public void ClearSubscribers()
		{
			Delegate.RemoveAll(StartEventDelegate, StartEventDelegate);
		}

		public void RaiseEvent()
		{
		}
	}
}