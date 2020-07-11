using CrystalClear.EventSystem;
using System;
using System.Collections;

namespace ScriptUtilities.StepRoutines
{
	public class StepRoutineInfo
	{
		public readonly int StepRoutineId;

		public readonly string StepRoutineName;

		public IEnumerator StepRoutine
		{
			get
			{
				stepRoutineWeakRef.TryGetTarget(out var obj);

				return obj;
			}
		}

		private readonly WeakReference<IEnumerator> stepRoutineWeakRef = new WeakReference<IEnumerator>(null);

		public StepRoutineInfo(int stepRoutineId, string stepRoutineName, IEnumerator stepRoutine)
		{
			StepRoutineId = stepRoutineId;
			StepRoutineName = stepRoutineName;
			stepRoutineWeakRef.SetTarget(stepRoutine);
		}
	}
}
