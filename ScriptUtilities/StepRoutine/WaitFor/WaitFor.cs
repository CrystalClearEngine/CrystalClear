using System;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public abstract class WaitFor
	{
		// TODO: should this be allowd to be inherited from outside the assembly?
		// Internal to prevent inheritance from outside the assembly.
		internal WaitFor()
		{

		}

		public abstract void Start(StepRoutineInfo stepRoutine);

		/// <summary>
		/// Cancel does not guarantee cancellation, in rare cases race conditions could prevent the cancellation measures from being effective.
		/// </summary>
		public abstract void Cancel();

		internal abstract void Cleanup();
	}
}
