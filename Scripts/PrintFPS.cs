using CrystalClear.Standard.Events;
using System;

namespace Scripts
{
	public static class FPSPrinter
	{
		[OnFrameUpdate]
		public static void PrintFPS()
		{
			Console.WriteLine(Math.Round(1 / FrameUpdateEvent.DeltaTimeInSeconds) + " FPS");
		}

		[OnInputPoll]
		public static void PrintPollCPS()
		{
			Console.WriteLine(Math.Round(1 / InputPollEvent.DeltaTimeInSeconds) + " IPPS");
		}

		[OnPhysicsTimeStep]
		public static void PrintPhysCPS()
		{
			Console.WriteLine(Math.Round(1 / PhysicsTimeStepEvent.DeltaTimeInSeconds) + " PUPS");
		}
	}
}
