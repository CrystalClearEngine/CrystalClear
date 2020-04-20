using CrystalClear.ScriptUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrystalClear.Standard.Events
{
	public static class EventStarter
	{
		[OnStartEvent]
		public static void StartEvents()
		{
			// Create a thread for updating the frame.
			Thread frameUpdateThread = new Thread(FrameUpdateEvent.FrameUpdateLoop);
			// Start the thread.
			frameUpdateThread.Start();

			// Create a thread for updating the physics' time-step.
			Thread physicsUpdateThread = new Thread(() => PhysicsTimeStepEventClass.PhysicsTimeStepLoop());
			// Start the thread.
			physicsUpdateThread.Start();

			// Create a thread for polling input.
			Thread inputPollingThread = new Thread(() => InputPollEvent.InputPollLoop());
			// Start the thread.
			inputPollingThread.Start();
		}
	}
}
