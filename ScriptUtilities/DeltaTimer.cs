using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace CrystalClear.ScriptUtilities
{
	public static class DeltaTimer
	{
		private static ConcurrentDictionary<string, Stopwatch> deltaTimers = new ConcurrentDictionary<string, Stopwatch>();

		public static double GetMethodUniqueDeltaTime([CallerMemberName] string methodName = null, [CallerFilePath] string callerFile = null)
		{
			return GetDeltaTime(methodName + callerFile);
		}

		public static double GetDeltaTime(string deltaTimerID)
		{
			if (!deltaTimers.ContainsKey(deltaTimerID))
			{
				Stopwatch newStopWatch = new Stopwatch();
				newStopWatch.Start();

				deltaTimers.TryAdd(deltaTimerID, newStopWatch);
			}

			Stopwatch stopwatch = deltaTimers[deltaTimerID];

			return stopwatch.Elapsed.TotalSeconds;
		}

		public static void RestartMethodUniqueDeltaTimer([CallerMemberName] string methodName = null, [CallerFilePath] string callerFile = null)
		{
			RestartDeltaTimer(methodName + callerFile);
		}

		public static void RestartDeltaTimer(string deltaTimerID)
		{
			Stopwatch stopwatch = deltaTimers[deltaTimerID];

			stopwatch.Restart();
		}
	}
}
