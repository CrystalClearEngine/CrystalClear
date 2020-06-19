using CrystalClear.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CrystalClear.ScriptUtilities
{
	public static class StepRoutine
	{
		// TODO: Should StepRoutines be IEnumerators or IEnumerables?
		// TODO: StepRoutineManager to manage all running StepRoutines.

		public static void Start(IEnumerator stepRoutine)
		{
			// Initialize stepRoutineRunnerDelegate.
			ScriptEventHandler stepRoutineRunnerDelegate = new ScriptEventHandler(() => { });
			// Assign the action.
			stepRoutineRunnerDelegate = new ScriptEventHandler(() => RunStepRoutine(stepRoutine, stepRoutineRunnerDelegate));
			// Start the StepRoutine.
			stepRoutine.MoveNext();
			// Subscribe the stepRoutineRunnerDelegate to the current WaitFor.
			((WaitFor)stepRoutine.Current).ScriptEvent.Subscribe(stepRoutineRunnerDelegate);
		}

		public static void Start(IEnumerator<WaitFor> stepRoutine)
		{
			try
			{
				// Initialize stepRoutineRunnerDelegate.
				ScriptEventHandler stepRoutineRunnerDelegate = new ScriptEventHandler(() => { });
				// Assign the action.
				stepRoutineRunnerDelegate = new ScriptEventHandler(() => RunStepRoutine(stepRoutine, stepRoutineRunnerDelegate));
				// Start the StepRoutine.
				stepRoutine.MoveNext();
				// Subscribe the stepRoutineRunnerDelegate to the current WaitFor.
				(stepRoutine.Current).ScriptEvent.Subscribe(stepRoutineRunnerDelegate);

				// Dispose of the StepRoutine.
				stepRoutine.Dispose();
			}
			finally
			{
				// Dispose of the StepRoutine.
				stepRoutine.Dispose();
			}
		}

		private static void RunStepRoutine(IEnumerator stepRoutine, ScriptEventHandler stepRoutineRunnerDelegate)
		{
			// Unsubscribe this delegate so it won't run again.
			((WaitFor)stepRoutine.Current).ScriptEvent.Unsubscribe(stepRoutineRunnerDelegate);
			// Move the enumerator forwards.
			if (stepRoutine.MoveNext())
			{
				// Subscribe the delegate to the new WaitFor ScriptEvent.
				((WaitFor)stepRoutine.Current).ScriptEvent.Subscribe(stepRoutineRunnerDelegate);
			}
		}
	}

	public class WaitFor // TODO: create separate WaitForScriptEvent and keep this as a base.
	{
		public ScriptEventBase ScriptEvent;

		public WaitFor(Type scriptEventType)
		{
			ScriptEvent =
				(ScriptEventBase)scriptEventType
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null); // TODO: add an ISingleton<maybe T> that we can then do GetInstance from instead of this.
		}

		public WaitFor(ScriptEventBase scriptEvent)
		{
			ScriptEvent = scriptEvent;
		}
	}
}
