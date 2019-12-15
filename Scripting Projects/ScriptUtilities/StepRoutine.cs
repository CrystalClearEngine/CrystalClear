﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using CrystalClear.EventSystem;
using System.Reflection;
using System.Threading;

namespace CrystalClear.ScriptUtilities
{
	public static class StepRoutine
	{
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
		public ScriptEvent ScriptEvent;

		public WaitFor(Type scriptEventType)
		{
			ScriptEvent =
				(ScriptEvent)scriptEventType
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null); // TODO: add an ISingleton<maybe T> that we can then do GetInstance from instead of this.
		}

		public WaitFor(ScriptEvent scriptEvent)
		{
			ScriptEvent = scriptEvent;
		}
	}
}