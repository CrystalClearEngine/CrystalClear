using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// A class that contains methods and implementation details for events that use EventHandler.
	/// </summary>
	/// <typeparam name="TEventArgs">The EventArgs to use in the EventHandler.</typeparam>
	public abstract class EventArgsScriptEvent<TEventArgs> : ScriptEvent
	where TEventArgs : EventArgs
	{
		/// <summary>
		/// The event.
		/// </summary>
		public event EventHandler<TEventArgs> Event;

		/// <summary>
		/// Gets the subscribers of the event.
		/// </summary>
		/// <returns>The subscribed delegates.</returns>
		public override Delegate[] Subscribers => Event.GetInvocationList();

		/// <summary>
		/// Raises the event.
		/// </summary>
		/// <param name="args">The arguments for this event.</param>
		public virtual void RaiseEvent(EventArgs args = null, object sender = null)
		{
			if (args == null)
			{
				args = EventArgs.Empty;
			}

			Event?.Invoke(sender, (TEventArgs)args);
		}

		/// <summary>
		/// Subscribes a delegate (that can cast to StandardEventHandler) to the event.
		/// </summary>
		/// <param name="toSubscribe">The delegate to subscribe.</param>
		public override void Subscribe(Delegate toSubscribe)
		{
			Event += (EventHandler<TEventArgs>)toSubscribe;
		}

		/// <summary>
		/// Subscribes a method to the event using a MethodInfo and instance. Utilizes CreateDelegate in MethodInfo.
		/// </summary>
		/// <param name="method">The method to subscribe.</param>
		/// <param name="instance">The method's class insance to subscribe to. (Null if the method is static)</param>
		public override void Subscribe(MethodInfo method, object instance)
		{
			Delegate @delegate = method.CreateDelegate(typeof(EventHandler<TEventArgs>), instance);
			Event += (EventHandler<TEventArgs>)@delegate;
		}

		/// <summary>
		/// Unsubscribes a delegate from the event.
		/// </summary>
		/// <param name="toUnsubscribe">The delegate to unsubscribe.</param>
		public override void Unsubscribe(Delegate toUnsubscribe)
		{
			Event -= (EventHandler<TEventArgs>)toUnsubscribe;
		}
	}
}
