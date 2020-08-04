using System;
using System.Reflection;
using CrystalClear.EventSystem;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	/// <summary>
	///     Waits for an event to be raised, then proceeds the StepRoutine.
	/// </summary>
	public sealed class WaitForEvent : WaitFor
	{
		private readonly ScriptEventBase eventToWaitFor;

		private ScriptEventHandler
			proceeder; // A delegate that will simply call ProceedStepRoutine. It is a field so that Cancel and Cleanup can access it.

		public WaitForEvent(Type scriptEventType)
		{
			eventToWaitFor =
				(ScriptEventBase) scriptEventType // TODO: make this into a utility method somewhere.
					.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
					.GetValue(null); // TODO: add an ISingleton<maybe T> that we can then do GetInstance from instead of this.
		}

		public WaitForEvent(ScriptEventBase scriptEvent)
		{
			eventToWaitFor = scriptEvent;
		}

		public override void Start(StepRoutineInfo stepRoutine)
		{
			/* To achieve the effect we want, we should create a delegate that calls ProceedStepRoutine.
			 * We subscribe that delegate to the event.
			 * When the event is raised ProceedStepRoutine will be called, then Cleanup will be called
			 * from there and the StepRoutine will proceed.
			*/

			// Create the proceeder delegate.
			proceeder = delegate { StepRoutine.ProceedStepRoutine(stepRoutine); };

			// Subscribe the delegate to the event so it will be called when the event is raised.
			eventToWaitFor.Subscribe(proceeder);
		}

		public override void Cancel()
		{
			if (proceeder != null)
				eventToWaitFor.Unsubscribe(proceeder);
		}

		internal override void Cleanup()
		{
			if (proceeder != null)
				eventToWaitFor.Unsubscribe(proceeder);
		}
	}
}