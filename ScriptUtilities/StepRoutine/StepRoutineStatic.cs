using System.Collections;

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

			ProceedStepRoutine(info);

			return info;
		}

		internal static void ProceedStepRoutine(StepRoutineInfo stepRoutine)
		{
			stepRoutine.CurrentWaitFor?.Cleanup();

			if (stepRoutine.State == StepRoutineState.AttemptedStop)
				return;

			if (stepRoutine.StepRoutineEnumerable.MoveNext())
				stepRoutine.CurrentWaitFor.Start(stepRoutine);
			else
			{
				stepRoutine.State = StepRoutineState.Finished;
				return;
			}
		}
	}
}
