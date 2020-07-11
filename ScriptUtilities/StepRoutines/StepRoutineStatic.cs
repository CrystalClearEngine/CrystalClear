using CrystalClear.EventSystem;
using CrystalClear.ScriptUtilities.StepRoutines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public static class StepRoutine
	{
		// TODO: Should StepRoutines be IEnumerators or IEnumerables?
		// TODO: support IEnumerators by just calling the regular methods with IEnumerator.GetEnumerable()

		// TODO: rename to StartStepRoutine since it can be used as an extension method.
		public static StepRoutineInfo Start(this IEnumerator stepRoutine, string name = null)
		{
			StepRoutineInfo info = StepRoutineManager.RegisterNewStepRoutine(stepRoutine, name);

			// Initialize stepRoutineRunnerDelegate.
			ScriptEventHandler stepRoutineRunnerDelegate = new ScriptEventHandler(() => { });
			// Assign the action.
			stepRoutineRunnerDelegate = new ScriptEventHandler(() => RunStepRoutine(stepRoutine, stepRoutineRunnerDelegate));
			// Start the StepRoutine.
			stepRoutine.MoveNext();
			// Subscribe the stepRoutineRunnerDelegate to the current WaitFor.
			((WaitForEvent)stepRoutine.Current).ScriptEvent.Subscribe(stepRoutineRunnerDelegate);

			return info;
		}

		// TODO: fix, doesn't actually work right now, will just resubscribe same old stuff.
		private static void RunStepRoutine(this IEnumerator stepRoutine, ScriptEventHandler stepRoutineRunnerDelegate)
		{
			// Unsubscribe this delegate so it won't run again.
			((WaitForEvent)stepRoutine.Current).ScriptEvent.Unsubscribe(stepRoutineRunnerDelegate);
			// Move the enumerator forwards.
			if (stepRoutine.MoveNext())
			{
				// Subscribe the delegate to the new WaitFor ScriptEvent.
				((WaitForEvent)stepRoutine.Current).ScriptEvent.Subscribe(stepRoutineRunnerDelegate);
			}
		}
	}

	public abstract class WaitFor
	{
		// Internal to prevent inheritance from outside the assembly.
		internal WaitFor()
		{

		}

		public abstract void Cancel();
	}

	public class WaitForEvent : WaitFor
	{
		public ScriptEventBase ScriptEvent;

		public ScriptEventHandler ScriptEventHandler;

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
			ScriptEvent.Unsubscribe(ScriptEventHandler);
		}
	}
}
