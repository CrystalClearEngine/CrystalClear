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
		/// <param name="method"></param>
		/// <param name="instance"></param>
		public abstract void Subscribe(MethodInfo method, object instance);

		/// <summary>
		/// Subscribes a delegate to the event.
		/// </summary>
		/// <param name="toSubscribe"></param>
		public abstract void Subscribe(Delegate toSubscribe);

		/// <summary>
		/// Unsubscribes a delegate from the event.
		/// </summary>
		/// <param name="toUnsubscribe"></param>
		public abstract void Unsubscribe(Delegate toUnsubscribe);

		/// <summary>
		/// Returns all delegates that are subscribed to this event.
		/// </summary>
		/// <returns>The subscribed delegates.</returns>
		public abstract Delegate[] GetSubscribers();
	}
}
