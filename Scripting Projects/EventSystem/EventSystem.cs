using System;

namespace CrystalClear.EventSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SubscribeToAttribute : Attribute
	{
		public IEvent Event; // The event *instance* to subscribe this method to

		public SubscribeToAttribute(Type eventType)
		{
			IEvent iEvent = (IEvent)Activator.CreateInstance(eventType);
			Event = iEvent.EventInstance;
		}
	}
}