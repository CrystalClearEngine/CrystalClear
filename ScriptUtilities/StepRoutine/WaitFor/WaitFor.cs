using System;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public abstract class WaitFor
	{
		// Internal to prevent inheritance from outside the assembly.
		internal WaitFor()
		{

		}

		protected event Action Continue; // The actions to perform when the wait is done

		/// <summary>
		/// Cancel does not guarantee cancellation, in rare cases race conditions could prevent the cancellation measures from being effective.
		/// </summary>
		public abstract void Cancel();

		public abstract void Start();
	}
}
