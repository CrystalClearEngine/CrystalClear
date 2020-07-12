using CrystalClear.EventSystem;
using CrystalClear.ScriptUtilities.StepRoutines;
using System.Collections;
using System.Collections.Generic;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public static class StepRoutine
	{
		// TODO: Should StepRoutines be IEnumerators or IEnumerables?
		// TODO: support IEnumerators by just calling the regular methods with IEnumerator.GetEnumerable()

		public static StepRoutineInfo StartStepRoutine(this IEnumerator stepRoutine, string name = null)
		{
			StepRoutineInfo info = StepRoutineManager.RegisterNewStepRoutine(stepRoutine, name);

			info.State = StepRoutineState.Running;

			DoStepInStepRoutine(info, null);

			return info;
		}

		// TODO: support multiple different WaitFor types.
		private static void DoStepInStepRoutine(this StepRoutineInfo stepRoutine, ScriptEventHandler previousStepRoutineRunnerDelegate)
		{
			// Unsubscribe the previous delegate so it won't run again.
			if (!(previousStepRoutineRunnerDelegate is null))
				((WaitForEvent)stepRoutine.StepRoutineEnumerable.Current).ScriptEvent.Unsubscribe(previousStepRoutineRunnerDelegate);

			// Move the enumerator forwards.
			if (stepRoutine.StepRoutineEnumerable.MoveNext())
			{
				previousStepRoutineRunnerDelegate = new ScriptEventHandler(() => DoStepInStepRoutine(stepRoutine, previousStepRoutineRunnerDelegate));
				((WaitForEvent)stepRoutine.StepRoutineEnumerable.Current).ScriptEvent.Subscribe(previousStepRoutineRunnerDelegate);
				((WaitForEvent)stepRoutine.StepRoutineEnumerable.Current).ScriptEventHandler = previousStepRoutineRunnerDelegate;
			}
			else
			{
				stepRoutine.State = StepRoutineState.Finished;
			}
		}
	}
}
