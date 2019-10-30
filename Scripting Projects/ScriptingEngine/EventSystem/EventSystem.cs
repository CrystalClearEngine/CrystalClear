using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	public interface IEventClass
	{
		void Subscribe(Delegate eventHandler);

		void Subscribe(MethodInfo method, object instance);

		void UnSubscribe(Delegate eventHandler);

		void RaiseEvent();

		void ClearSubscribers();
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