using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	public abstract class ScriptEvent
	{
		// Methods.
		public abstract void RaiseEvent(EventArgs args);

		public abstract void Subscribe(MethodInfo method, object instance);

		public abstract void Subscribe(Delegate toSubscribe);

		public abstract void Unsubscribe(Delegate toUnsubscribe);

		public abstract Delegate[] GetSubscribers();
	}

	public abstract class TemplateScriptEvent<TEventArgs> : ScriptEvent
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
		public override void RaiseEvent(EventArgs args)
		{
			Event(this, (TEventArgs)args);
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

	public abstract class SingletonScriptEvent<T, TEventArgs> : TemplateScriptEvent<TEventArgs>
		where T : SingletonScriptEvent<T, TEventArgs>, new()
		where TEventArgs : EventArgs
	{
		// Singleton stuff.
		// Static constructor to ensure that any SingletonScriptEvent will be created at runtime unless overriden in a deriving class.
		static SingletonScriptEvent()
		{
		}

		// Protected constructor for new T() and deriving classes.
		protected SingletonScriptEvent()
		{
		}

		// The instance.
		private static T _instance;

		// The instance's public property.
		public static T Instance => _instance ?? (_instance = new T());
	}
}
