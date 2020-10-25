using CrystalClear;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scripts
{
	[IsScript]
	public static class DeltaTimerTest
	{
		[OnFrameUpdate]
		public static void FastMethodUnique()
		{
			Output.Log(Math.Round(DeltaTimer.GetDeltaTime("FastMethodUnique"), 5));
			DeltaTimer.RestartDeltaTimer("FastMethodUnique");
		}

		[OnPhysicsTimeStep]
		public static void PredictableMethodUnique()
		{
			Output.Log(Math.Round(DeltaTimer.GetDeltaTime("FastMethodUnique"), 5));
			DeltaTimer.RestartDeltaTimer("PredictableMethodUnique");
		}

		[OnInputPoll]
		public static void SlowMethodUnique()
		{
			Thread.Sleep(500);
			Output.Log(Math.Round(DeltaTimer.GetDeltaTime("FastMethodUnique"), 5));
			DeltaTimer.RestartDeltaTimer("SlowMethodUnique");
		}
	}
}
