using CrystalClear.EventSystem;
using System;
using System.Collections;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public class StepRoutineInfo
	{
		// TODO: add TryStop method.

		public readonly int StepRoutineId;

		public readonly string StepRoutineName;

		public IEnumerator StepRoutineEnumerable
		{
			get
			{
				stepRoutineWeakRef.TryGetTarget(out var obj);

				return obj;
			}
		}

		private readonly WeakReference<IEnumerator> stepRoutineWeakRef = new WeakReference<IEnumerator>(null);

		public StepRoutineState State { get; internal set; } = StepRoutineState.NotStarted;

		public WaitFor CurrentWaitFor => (WaitFor)StepRoutineEnumerable.Current;

		public StepRoutineInfo(int stepRoutineId, string stepRoutineName, IEnumerator stepRoutine)
		{
			StepRoutineId = stepRoutineId;
			StepRoutineName = stepRoutineName;
			stepRoutineWeakRef.SetTarget(stepRoutine);
		}

		public void TryStop()
		{
			State = StepRoutineState.AttemptedStop;
			CurrentWaitFor.Cancel();
		}

		public void Resume()
		{
			CurrentWaitFor.Start();
			State = StepRoutineState.Running;
		}
	}

	public enum StepRoutineState
	{
		NotStarted,
		Running,
		Finished,
		AttemptedStop,
	}
}
