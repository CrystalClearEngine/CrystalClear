using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public static class StepRoutineManager
	{
		private static readonly Dictionary<int, StepRoutineInfo> runningStepRoutines =
			new Dictionary<int, StepRoutineInfo>();

		private static readonly Dictionary<string, int> nameToStepRoutineIdTranslator = new Dictionary<string, int>();

		private static readonly Random random = new Random();

		public static StepRoutineInfo GetStepRoutine(int id) => runningStepRoutines[id];

		public static StepRoutineInfo GetStepRoutine(string name) =>
			runningStepRoutines[nameToStepRoutineIdTranslator[name]];

		public static void StopAllStepRoutines()
		{
			throw new NotImplementedException();
		}

		// TODO: Ensure that this is correct, that IEnumerators that run are equal to one not run etc.
		public static void StopAllOfType(IEnumerator stepRoutineTypeToStop)
		{
			IEnumerable<StepRoutineInfo> toStop = from StepRoutineInfo info in runningStepRoutines.Values
				where info.StepRoutineEnumerable.Equals(stepRoutineTypeToStop)
				select info;

			foreach (StepRoutineInfo stepRoutine in toStop)
			{
				stepRoutine.TryStop();
			}
		}

		public static IEnumerable<StepRoutineInfo> EnumerateStepRoutines() => runningStepRoutines.Values;

		/// <summary>
		///     Registers a new StepRoutine, adding it to the database.
		/// </summary>
		/// <returns>The new StepRoutine's ID.</returns>
		internal static StepRoutineInfo RegisterNewStepRoutine(IEnumerator stepRoutine, string name = null)
		{
			var id = GenerateNewId(name);

			StepRoutineInfo stepRoutineInfo;

			if (name is not null)
				nameToStepRoutineIdTranslator.Add(name, id);

			stepRoutineInfo = new StepRoutineInfo(id, name, stepRoutine);

			runningStepRoutines.Add(id, stepRoutineInfo);

			return stepRoutineInfo;
		}

		private static int GenerateNewId(string name)
		{
			var proposedId = 0;

			do
			{
				if (proposedId == 0)
				{
					proposedId += random.Next();
				}
				else if (name is not null)
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