using System;

namespace ScriptUtilities.StepRoutines
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class StepRoutineAttribute : Attribute
	{
		// TODO: find simpler name?
		public bool AllowMultipleToRunAtSameTime = true;

		public StepRoutineAttribute()
		{

		}
	}
}
