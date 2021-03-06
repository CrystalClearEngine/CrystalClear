﻿using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	// TODO: add FromType method that does the reflection stuff?
	public abstract class ScriptEventBase
	{
		// TODO: make use generic type for delegate type?
		public event ScriptEventHandler Event;

		public virtual void RaiseEvent()
		{
			Event?.Invoke();
		}

		public Delegate[] GetSubscribers() => Event?.GetInvocationList();

		#region Subscription Management

		public void Subscribe(Delegate toSubscribe)
		{
			Event += (ScriptEventHandler) toSubscribe;
		}

		public void Subscribe(MethodInfo method, object instance)
		{
			Delegate @delegate = method.CreateDelegate(typeof(ScriptEventHandler), instance);
			Event += (ScriptEventHandler) @delegate;
		}

		public void Unsubscribe(Delegate toUnsubscribe)
		{
			Event -= (ScriptEventHandler) toUnsubscribe;
		}

		#endregion
	}

	public abstract class ScriptEvent<TInstance>
		: ScriptEventBase
		where TInstance : new()
	{
		private static TInstance _instance;

		// TODO: use other thread safe method.
		public static TInstance Instance => _instance ??= new TInstance();
	}
}