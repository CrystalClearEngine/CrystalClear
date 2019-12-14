using CrystalClear.EventSystem;
using System;
using System.Reflection;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The test event event class. This event is used for testing purposes.
	/// </summary>
	public class TestEventClass : SingletonScriptEventHandlerScriptEvent<TestEventClass>
	{
		public override void RaiseEvent()
		{
			Console.WriteLine("Raising test event.");
			base.RaiseEvent();
			Console.WriteLine("Raised test event.");
		}

		public override void Subscribe(Delegate toSubscribe)
		{
			Console.WriteLine($"Subscribing delegate to test event: {toSubscribe.Method.Name}");
			base.Subscribe(toSubscribe);
			Console.WriteLine($"Subscribed delegate to test event: {toSubscribe.Method.Name}");
		}

		public override void Subscribe(MethodInfo method, object instance)
		{
			Console.WriteLine($"Subscribing delegate to test event: {method.Name}");
			base.Subscribe(method, instance);
			Console.WriteLine($"Subscribed delegate to test event: {method.Name}");
		}

		public override void Unsubscribe(Delegate toUnsubscribe)
		{
			Console.WriteLine($"Unsubscribing delegate from test event: {toUnsubscribe.Method.Name}");
			base.Unsubscribe(toUnsubscribe);
			Console.WriteLine($"Unsubscribed delegate from test event: {toUnsubscribe.Method.Name}");
		}
	}
}