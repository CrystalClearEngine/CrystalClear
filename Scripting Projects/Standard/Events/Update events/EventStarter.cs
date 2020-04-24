using CrystalClear.ScriptUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CrystalClear.EventSystem.StandardEvents;

namespace CrystalClear.Standard.Events
{
	public static class EventStarter
	{
		[OnStartEvent]
		public static void StartEvents()
		{
			// Start the FrameUpdateEvent.
			FrameUpdateEvent.Start();

			// Start the InputPollEvent.
			InputPollEvent.Start();

			// Start the FrameUpdateEvent.
			PhysicsTimeStepEvent.Start(TimeSpan.FromSeconds(0.0166));
		}

		[OnStopEvent]
		public static void StopEvents()
		{
			FrameUpdateEvent.Stop();
			InputPollEvent.Stop();
			PhysicsTimeStepEvent.Stop();
		}
	}
}
