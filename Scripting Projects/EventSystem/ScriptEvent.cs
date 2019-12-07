using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// The base class for ScriptObjects. This contains the abstract methods but no implementation details.
	/// </summary>
	public abstract class ScriptEvent
	{
		// Methods.
		public abstract void RaiseEvent(EventArgs args = null, object sender = null);

		public abstract void Subscribe(MethodInfo method, object instance);

		public abstract void Subscribe(Delegate toSubscribe);

		public abstract void Unsubscribe(Delegate toUnsubscribe);

		public abstract Delegate[] GetSubscribers();
	}

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
		public override Delegate[] GetSubscribers()
		{
			return Event.GetInvocationList();
		}

		/// <summary>
		/// Raises the event.
		/// </summary>
		/// <param name="args">The arguments for this event.</param>
		public override void RaiseEvent(EventArgs args = null, object sender = null)
		{
			if (args == null)
				args = EventArgs.Empty;

			Event(sender, (TEventArgs)args);
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

	/// <summary>
	/// A singleton version of the EventArgsScriptEvent. Contains implementation for a singleton.
	/// </summary>
	/// <typeparam name="InstanceType">The type of the instance. Should generally be the same as the deriving class.</typeparam>
	/// <typeparam name="TEventArgs"></typeparam>
	public abstract class SingletonEventArgsScriptEvent<InstanceType, TEventArgs> : EventArgsScriptEvent<TEventArgs>
		where InstanceType : SingletonEventArgsScriptEvent<InstanceType, TEventArgs>, new()
		where TEventArgs : EventArgs
	{
		// Singleton stuff.

		// Static constructor to ensure that any SingletonScriptEvent will be created at runtime unless overriden in a deriving class.
		static SingletonEventArgsScriptEvent()
		{
		}

		// Protected constructor for new T() and deriving classes.
		protected SingletonEventArgsScriptEvent()
		{
		}

		// The instance.
		private static InstanceType _instance;

		// The instance's public property.
		public static InstanceType Instance => _instance ?? (_instance = new InstanceType());
	}
}
