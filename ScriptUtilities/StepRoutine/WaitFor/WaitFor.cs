namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public abstract class WaitFor
	{
		// Internal to prevent inheritance from outside the assembly.
		internal WaitFor()
		{

		}

		// TODO: add Resume() method?

		/// <summary>
		/// Cancel does not guarantee cancellation, in rare cases race conditions could prevent the cancellation measures from being effective.
		/// </summary>
		public abstract void Cancel();
	}
}
