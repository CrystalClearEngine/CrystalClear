using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// The base class for ScriptObjects. It contains the abstract methods but no implementation details.
	/// </summary>
	public abstract class ScriptEvent
	{
		// Methods.

		/// <summary>
		/// Subscribes a method to the event.
		/// </summary>
		/// <param name="method">The method to subscribe.</param>
		/// <param name="instance">The instance of the method's class to use.</param>
		public abstract void Subscribe(MethodInfo method, object instance);

		/// <summary>
		/// Subscribes a delegate to the event.
		/// </summary>
		/// <param name="toSubscribe">The delegate to subscribe.</param>
		public abstract void Subscribe(Delegate toSubscribe);

		/// <summary>
		/// Unsubscribes a delegate from the event.
		/// </summary>
		/// <param name="toUnsubscribe">The delegate to unsubscribe.</param>
		public abstract void Unsubscribe(Delegate toUnsubscribe);

		/// <summary>
		/// Returns all delegates that are subscribed to this event.
		/// </summary>
		/// <returns>The subscribed delegates.</returns>
		public abstract Delegate[] GetSubscribers();
	}
}
