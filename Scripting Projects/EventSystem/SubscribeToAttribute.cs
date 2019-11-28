using System;

namespace CrystalClear.EventSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SubscribeToAttribute : Attribute
	{
		/// <summary>
		/// The event instance supplied to the attribute.
		/// </summary>
		public IEvent Event;

		public SubscribeToAttribute(Type eventType)
		{
			IEvent iEvent = (IEvent)Activator.CreateInstance(eventType);
			Event = iEvent.EventInstance;
		}
	}
}