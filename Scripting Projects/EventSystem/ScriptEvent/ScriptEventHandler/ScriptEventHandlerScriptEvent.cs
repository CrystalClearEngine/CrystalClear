using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// A class that contains methods and implementation details for events that use the ScriptEventHandler.
	/// </summary>
	public abstract class ScriptEventHandlerScriptEvent : ScriptEvent
	{
		/// <summary>
		/// The event.
		/// </summary>
		public event ScriptEventHandler Event;

		/// <summary>
		/// Gets the subscribers of the event.
		/// </summary>
		/// <returns>The subscribed delegates.</returns>
		public override Delegate[] GetSubscribers()
		{
			return Event?.GetInvocationList();
		}

		/// <summary>
		/// Raises the event.
		/// </summary>
		public virtual void RaiseEvent()
		{
			Event?.Invoke();
		}

		/// <summary>
		/// Subscribes a delegate (that can cast to StandardEventHandler) to the event.
		/// </summary>
		/// <param name="toSubscribe">The delegate to subscribe.</param>
		public override void Subscribe(Delegate toSubscribe)
		{
			Event += (ScriptEventHandler)toSubscribe;
		}

		/// <summary>
		/// Subscribes a method to the event using a MethodInfo and instance. Utilizes CreateDelegate in MethodInfo.
		/// </summary>
		/// <param name="method">The method to subscribe.</param>
		/// <param name="instance">The method's class insance to subscribe to. (Null if the method is static.)</param>
		public override void Subscribe(MethodInfo method, object instance)
		{
			Delegate @delegate = method.CreateDelegate(typeof(ScriptEventHandler), instance);
			Event += (ScriptEventHandler)@delegate;
		}

		/// <summary>
		/// Unsubscribes a delegate from the event.
		/// </summary>
		/// <param name="toUnsubscribe">The delegate to unsubscribe.</param>
		public override void Unsubscribe(Delegate toUnsubscribe)
		{
			Event -= (ScriptEventHandler)toUnsubscribe;
		}
	}
}
