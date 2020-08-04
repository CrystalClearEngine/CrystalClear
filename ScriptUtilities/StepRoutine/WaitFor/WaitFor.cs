namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public abstract class WaitFor
	{
		// TODO: should this be allowed to be inherited from outside the assembly?
		// Internal to prevent inheritance from outside the assembly.
		internal WaitFor()
		{
		}

		// TODO: couldn't this stuff just be done in the constructor? It wouldn't allow a Resume(), but perhaps the constructor should call this?
		public abstract void Start(StepRoutineInfo stepRoutine);

		/// <summary>
		///     Cancel does not guarantee cancellation, in rare cases race conditions could prevent the cancellation measures from
		///     being effective.
		/// </summary>
		public abstract void Cancel();

		internal abstract void Cleanup();
	}
}