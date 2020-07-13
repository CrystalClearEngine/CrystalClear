using CrystalClear.EventSystem;
using System;
using System.Reflection;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public class WaitForEvent : WaitFor
	{
		public ScriptEventBase ScriptEvent;

		internal Action Action;

		public WaitForEvent(Type scriptEventType)
		{
			ScriptEvent =
				(ScriptEventBase)scriptEventType
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null); // TODO: add an ISingleton<maybe T> that we can then do GetInstance from instead of this.
		}

		public WaitForEvent(ScriptEventBase scriptEvent)
		{
			ScriptEvent = scriptEvent;
		}

		public override void Cancel()
		{
			ScriptEvent.Unsubscribe(Action);
		}

		public override void Start()
		{
			throw new NotImplementedException();
		}
	}
}
