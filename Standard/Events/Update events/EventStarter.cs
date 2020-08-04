using System;

namespace CrystalClear.Standard.Events
{
	public static class EventStarter
	{
		public static void StartEvents(
			TimeSpan frameUpdateEventInterval = new TimeSpan(),
			TimeSpan inputPollEventInterval = new TimeSpan(),
			TimeSpan physicsTimeStepEventInterval = new TimeSpan())
		{
			// Start the FrameUpdateEvent.
			FrameUpdateEvent.Start(frameUpdateEventInterval);

			// Start the InputPollEvent.
			InputPollEvent.Start(inputPollEventInterval);

			// Start the FrameUpdateEvent.
			PhysicsTimeStepEvent.Start(physicsTimeStepEventInterval);
		}

		public static void StopEvents()
		{
			FrameUpdateEvent.Stop();
			InputPollEvent.Stop();
			PhysicsTimeStepEvent.Stop();
		}
	}
}