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
		public static Thread frameUpdateThread;
		public static Thread physicsUpdateThread;
		public static Thread inputPollingThread;

		[OnStartEvent]
		public static void StartEvents()
		{
			// Create a thread for updating the frame.
			frameUpdateThread = new Thread(FrameUpdateEvent.FrameUpdateLoop);
			// Start the thread.
			frameUpdateThread.Start();

			// Create a thread for updating the physics' time-step.
			physicsUpdateThread = new Thread(() => PhysicsTimeStepEventClass.PhysicsTimeStepLoop());
			// Start the thread.
			physicsUpdateThread.Start();

			// Create a thread for polling input.
			inputPollingThread = new Thread(() => InputPollEvent.InputPollLoop());
			// Start the thread.
			inputPollingThread.Start();
		}

		public static void StopEvents()
		{
			frameUpdateThread.Abort();
			physicsUpdateThread.Abort();
			inputPollingThread.Abort();
		}
	}
}
