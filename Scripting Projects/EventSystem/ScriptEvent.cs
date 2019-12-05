﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.EventSystem
{
	public delegate void EventDelegateType();

	public class ScriptEvent : Singleton<ScriptEvent>
	{
		public virtual void RaiseEvent(params object[] raiseParameters)
		{
			throw new NotSupportedException();
		}

		public virtual void Subscribe(System.Reflection.MethodInfo method, object instance)
		{
			throw new NotSupportedException();
		}

		public virtual void Subscribe(Delegate toSubscribe)
		{
			throw new NotSupportedException();
		}

		public virtual void Unsubscribe(Delegate toUnsubscribe)
		{
			throw new NotSupportedException();
		}

		public virtual Delegate[] GetSubscribers()
		{
			throw new NotSupportedException();
		}
	}
}
