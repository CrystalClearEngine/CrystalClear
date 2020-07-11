using System;
using System.Collections;
using System.Collections.Generic;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public static class StepRoutineManager
	{
		private static Dictionary<int, StepRoutineInfo> runningStepRoutines = new Dictionary<int, StepRoutineInfo>();

		private static Dictionary<string, int> nameToStepRoutineIdTranslator = new Dictionary<string, int>();

		private static Random random = new Random();

		public static StepRoutineInfo GetStepRoutine(int id)
		{
			return runningStepRoutines[id];
		}
		
		/// <summary>
		/// Registers a new StepRoutine, adding it to the database.
		/// </summary>
		/// <returns>The new StepRoutine's ID.</returns>
		internal static StepRoutineInfo RegisterNewStepRoutine(IEnumerator stepRoutine, string name = null)
		{
			int id = GenerateNewId(name);

			StepRoutineInfo stepRoutineInfo;

			if (name != null)
				nameToStepRoutineIdTranslator.Add(name, id);

			stepRoutineInfo = new StepRoutineInfo(id, name, stepRoutine);

			runningStepRoutines.Add(id, stepRoutineInfo);

			return stepRoutineInfo;
		}

		private static int GenerateNewId(string name)
		{
			int proposedId = 0;

			do
			{
				if (proposedId == 0)
				{
					proposedId += random.Next();
				}
				else if (name != null)
				{
					proposedId = runningStepRoutines.Count + 1;
				}
				else
				{
					proposedId = name.GetHashCode();
				}
			} while (runningStepRoutines.ContainsKey(proposedId));

			return proposedId;
		}
	}
}
