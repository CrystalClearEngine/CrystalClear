using CrystalClear.EventSystem;
using System;
using System.Reflection;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	/// <summary>
	/// Waits for an event to be raised, then proceeds the StepRoutine.
	/// </summary>
	public sealed class WaitForEvent : WaitFor
	{
		ScriptEventBase EventToWaitFor;

		public WaitForEvent(Type scriptEventType)
		{
			EventToWaitFor =
				(ScriptEventBase)scriptEventType // TODO: make this into a utility method somewhere.
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null); // TODO: add an ISingleton<maybe T> that we can then do GetInstance from instead of this.
		}

		public WaitForEvent(ScriptEventBase scriptEvent)
		{
			EventToWaitFor = scriptEvent;
		}
	}
}
