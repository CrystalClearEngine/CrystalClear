using CrystalClear.EventSystem;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CrystalClear.Standard.Events
{
	public class OnStartEventAttribute : SubscribeToAttribute
	{
		public OnStartEventAttribute() : base(typeof(StartEventClass))
		{
		}
	}

	public class StartEventClass : ScriptEvent
	{
		public event EventDelegateType Event;

		public override Delegate[] GetSubscribers()
		{
			return Event.GetInvocationList();
		}

		public override void RaiseEvent(params object[] raiseParameters)
		{
			if (raiseParameters != null)
				throw new Exception("No raise parameters necessary for this event.");

			Event();
		}

		public override void Subscribe(Delegate toSubscribe)
		{
			Event += (EventDelegateType)toSubscribe;
		}

		public override void Unsubscribe(Delegate toUnsubscribe)
		{
			Event -= (EventDelegateType)toUnsubscribe;
		}
	}
}