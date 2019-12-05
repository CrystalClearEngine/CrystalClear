using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	public delegate void StandardEventHandler();

	public abstract class ScriptEvent
	{
		// Methods.
		public abstract void RaiseEvent(params object[] raiseParameters);

		public abstract void Subscribe(System.Reflection.MethodInfo method, object instance);

		public abstract void Subscribe(Delegate toSubscribe);

		public abstract void Unsubscribe(Delegate toUnsubscribe);

		public abstract Delegate[] GetSubscribers();
	}

	public abstract class SingletonScriptEvent<T> : ScriptEvent where T : SingletonScriptEvent<T>, new()
	{
		// Singleton stuff.
		static SingletonScriptEvent()
		{
		}

		protected SingletonScriptEvent()
		{
		}

		private static T _instance;

		public static T Instance => _instance ?? (_instance = new T());
	}

	public abstract class StandardSingletonScriptEvent<T> : SingletonScriptEvent<T> where T : StandardSingletonScriptEvent<T>, new()
	{
		/// <summary>
		/// The event.
		/// </summary>
		public event StandardEventHandler Event;

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
		/// <param name="raiseParameters">Should not be provided as they are not needed for this type of event.</param>
		public override void RaiseEvent(params object[] raiseParameters)
		{
			if (raiseParameters.Length > 0)
				throw new Exception("No raise parameters necessary for this event.");

			Event();
		}

		/// <summary>
		/// Subscribes a delegate (that can cast to StandardEventHandler) to the event.
		/// </summary>
		/// <param name="toSubscribe">The delegate to subscribe.</param>
		public override void Subscribe(Delegate toSubscribe)
		{
			Event += (StandardEventHandler)toSubscribe;
		}

		/// <summary>
		/// Subscribes a method to the event using a MethodInfo and instance. Utilizes CreateDelegate in MethodInfo.
		/// </summary>
		/// <param name="method">The method to subscribe.</param>
		/// <param name="instance">The method´s class insance to subscribe to. (Null if the method is static)</param>
		public override void Subscribe(MethodInfo method, object instance)
		{
			Delegate @delegate = method.CreateDelegate(typeof(StandardEventHandler), instance);
			Event += (StandardEventHandler)@delegate;
		}

		/// <summary>
		/// Unsubscribes a delegate from the event.
		/// </summary>
		/// <param name="toUnsubscribe">The delegate to unsubscribe.</param>
		public override void Unsubscribe(Delegate toUnsubscribe)
		{
			Event -= (StandardEventHandler)toUnsubscribe;
		}
	}
}
