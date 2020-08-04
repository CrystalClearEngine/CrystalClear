using System;
using CrystalClear;
using CrystalClear.Standard.Events;

namespace Scripts
{
	public static class FPSPrinter
	{
		[OnFrameUpdate]
		public static void PrintFPS()
		{
			Output.Log(Math.Round(1 / FrameUpdateEvent.DeltaTimeInSeconds) + " FPS");
		}

		[OnInputPoll]
		public static void PrintPollCPS()
		{
			Output.Log(Math.Round(1 / InputPollEvent.DeltaTimeInSeconds) + " IPPS");
		}

		[OnPhysicsTimeStep]
		public static void PrintPhysCPS()
		{
			Output.Log(Math.Round(1 / PhysicsTimeStepEvent.DeltaTimeInSeconds) + " PUPS");
		}
	}
}