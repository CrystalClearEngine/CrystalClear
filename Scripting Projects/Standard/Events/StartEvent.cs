using System;
using System.Reflection;
using CrystalClear.EventSystem;

namespace CrystalClear.Standard.Events
{
	public class OnStartEventAttribute : SubscribeToAttribute
	{
		public OnStartEventAttribute()
		{
			Event = StartEventClass.StartEventInstance;
		}
	}

	public class StartEventClass : IEvent
	{
		private StartEventClass()
		{
		}

		static StartEventClass()
		{
			StartEventInstance = new StartEventClass();
		}

		public static IEvent StartEventInstance;

		public IEvent EventInstance
		{
			get
			{
				return StartEventInstance;
			}
		}

		public delegate void StartEventHandler();

		private StartEventHandler StartEventDelegate;


		public void Subscribe(Delegate eventHandler)
		{
			StartEventDelegate += (StartEventHandler) eventHandler;
		}

		public void Subscribe(MethodInfo method, object scriptInstance)
		{
			Delegate eventHandler = Delegate.CreateDelegate(typeof(StartEventHandler), scriptInstance, method);
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

		public void OnEvent()
		{
			StartEventDelegate();
		}
	}
}