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
}
