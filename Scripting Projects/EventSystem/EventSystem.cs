using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	public interface IEvent
	{
		IEvent EventInstance { get; }

		void Subscribe(Delegate delegateToSubscribe);

		void Subscribe(MethodInfo method, object scriptInstance);

		void UnSubscribe(Delegate eventHandler);

		void OnEvent();

		void ClearSubscribers();
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class SubscribeToAttribute : Attribute
	{
		public IEvent Event; // The type to subscribe this method to

		public SubscribeToAttribute(IEvent @event)
		{
			Event = @event;
		}

		protected SubscribeToAttribute() // Only for use in deriving attributes (see OnStartEventAttribute)
		{
		}
	}
}