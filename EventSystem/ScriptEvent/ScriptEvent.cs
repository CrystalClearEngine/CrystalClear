using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	public abstract class ScriptEvent<T>
		where T : new()
	{
		public event ScriptEventHandler Event;

		public void RaiseEvent()
		{
			Event?.Invoke();
		}

		public Delegate[] GetSubscribers()
		{
			return Event?.GetInvocationList();
		}

		#region Subscription Management
		public void Subscribe(Delegate toSubscribe)
		{
			Event += (ScriptEventHandler)toSubscribe;
		}

		public void Subscribe(MethodInfo method, object instance)
		{
			Delegate @delegate = method.CreateDelegate(typeof(ScriptEventHandler), instance);
			Event += (ScriptEventHandler)@delegate;
		}

		public void Unsubscribe(Delegate toUnsubscribe)
		{
			Event -= (ScriptEventHandler)toUnsubscribe;
		}
		#endregion

		protected ScriptEvent() // For "new T()" and for deriving classes.
		{
		}

		private static T _instance;

		public static T Instance => _instance ?? (_instance = new T());
	}
}
