using CrystalClear.EventSystem;
using System;
using System.Reflection;

namespace CrystalClear.Standard.Events
{
	public class OnStartEventAttribute : SubscribeToAttribute
	{
		public OnStartEventAttribute() : base(typeof(StartEventClass))
		{
		}
	}

	public class StartEventClass : IEvent
	{
		static StartEventClass() // The static constructor for this class, makes sure that there is an instance of the starteventinstance just waiting to be used!
		{
			StartEventInstance = new StartEventClass();
		}

		private StartEventClass()
		{
		}

		public static IEvent StartEventInstance;

		public delegate void StartEventHandler();

		private static StartEventHandler StartEventDelegate;

		#region IEventImplementation
		public IEvent EventInstance
		{
			get
			{
				return StartEventInstance;
			}
		}

		public void Subscribe(Delegate eventHandler)
		{
			StartEventDelegate += (StartEventHandler)eventHandler;
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
		#endregion
	}
}