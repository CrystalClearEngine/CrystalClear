using System;
using System.Reflection;
using CrystalClear.Scripting.EventSystem.Events;

namespace CrystalClear.Scripting.EventSystem
{
	public struct Event
	{
		public Event(Type eventType, MethodInfo method)
		{
			EventType = eventType;
			Method = method;
		}

		public void Subscribe(object instance)
		{
			Delegate eventHandler = Delegate.CreateDelegate(EventType, instance, Method);
			StartEventClass.StartEvent += eventHandler as StartEventHandler;
			//if (Delegate.CreateDelegate(EventType, instance, Method) is
			//	StartEventHandler startEventHandler)
		}

		public Type EventType;
		public MethodInfo Method;
	}

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
	
	public class CancellableEventArgs : EventArgs
	{
		public bool IsCancelled { get; protected set; }

		public void Cancel()
		{
			IsCancelled = true;
		}
	}
}