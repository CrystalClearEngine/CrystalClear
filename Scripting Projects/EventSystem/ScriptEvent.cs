using System;

namespace CrystalClear.EventSystem
{
	public delegate void EventDelegateType();

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
}
