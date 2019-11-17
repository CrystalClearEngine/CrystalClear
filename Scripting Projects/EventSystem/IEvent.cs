using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	public interface IEvent // This interface lets event implementors be creative and unrestricted when creating their own events, while letting the users of the events easily subscribe, unsubscribe, raise or otherwise modify the event in a safe way
	{
		IEvent EventInstance { get; }

		/// <summary>
		/// Lets you subscribe delegates to the event
		/// </summary>
		/// <param name="delegateToSubscribe">This is the delegate that should be subscribed to the event</param>
		void Subscribe(Delegate delegateToSubscribe);

		/// <summary>
		/// Subscribe(Delegate) but letting the implementor turn it into a delegate
		/// </summary>
		/// <param name="method">The MethodInfo to subscribe</param>
		/// <param name="scriptInstance">The instance of the script containing the method</param>
		void Subscribe(MethodInfo method, object scriptInstance);

		/// <summary>
		/// Lets you unsubscribe delegates from the event
		/// </summary>
		/// <param name="delegateToUnsubscribe">The delegate to unsubscribe</param>
		void UnSubscribe(Delegate delegateToUnsubscribe);

		/// <summary>
		/// Calls the event (without parameters or return values). To call with parameters or return values, use another interface inheriting IEvent
		/// </summary>
		void OnEvent();

		/// <summary>
		/// Clears all subscriptions
		/// </summary>
		void ClearSubscribers();
	}
}