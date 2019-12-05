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

	public class StartEventClass : SingletonScriptEvent<StartEventClass>
	{
		public event EventDelegateType Event;

		public override Delegate[] GetSubscribers()
		{
			return Event.GetInvocationList();
		}

		public override void RaiseEvent(params object[] raiseParameters)
		{
			if (raiseParameters.Length > 0
				)
				throw new Exception("No raise parameters necessary for this event.");

			Event();
		}

		public override void Subscribe(Delegate toSubscribe)
		{
			Event += (EventDelegateType)toSubscribe;
		}

		public override void Subscribe(MethodInfo method, object instance)
		{
			Delegate @delegate = method.CreateDelegate(typeof(EventDelegateType), instance);
			Event += (EventDelegateType)@delegate;
		}

		public override void Unsubscribe(Delegate toUnsubscribe)
		{
			Event -= (EventDelegateType)toUnsubscribe;
		}
	}
}