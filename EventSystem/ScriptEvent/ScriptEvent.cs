using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	public abstract class ScriptEventBase
	{
		public event ScriptEventHandler Event;

		public virtual void RaiseEvent()
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
	}

	public abstract class ScriptEvent<TInstance>
		: ScriptEventBase
		where TInstance : new()
	{
		protected ScriptEvent() // For "new T()" and for deriving classes.
		{
		}

		private static TInstance _instance;

		public static TInstance Instance => _instance ?? (_instance = new TInstance());
	}
}
