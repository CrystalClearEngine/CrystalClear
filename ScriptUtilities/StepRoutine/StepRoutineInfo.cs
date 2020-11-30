using System;
using System.Collections;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public class StepRoutineInfo
	{
		public readonly int StepRoutineId;

		public readonly string StepRoutineName;

		private readonly WeakReference<IEnumerator> stepRoutineWeakRef = new WeakReference<IEnumerator>(null);

		public StepRoutineInfo(int stepRoutineId, string stepRoutineName, IEnumerator stepRoutine)
		{
			StepRoutineId = stepRoutineId;
			StepRoutineName = stepRoutineName;
			stepRoutineWeakRef.SetTarget(stepRoutine);
		}

		public IEnumerator StepRoutineEnumerable => stepRoutineWeakRef.TryGetTargetExt();

		public StepRoutineState State { get; internal set; } = StepRoutineState.NotStarted;

		public WaitFor CurrentWaitFor => StepRoutineEnumerable?.Current as WaitFor;

		public void TryStop()
		{
			State = StepRoutineState.AttemptedStop;
			CurrentWaitFor?.Cancel();
			// TODO: make sure to remove from StepRoutineManager's list of RunningStepRoutines when this happens...
		}

		public void Resume()
		{
			// TODO: finish
			throw new NotImplementedException();
			//CurrentWaitFor.Start();
			//State = StepRoutineState.Running;
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