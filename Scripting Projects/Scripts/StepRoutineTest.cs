using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Collections;

namespace Scripts
{
	[IsScript]
	public class StepRoutineTest : HierarchyScript<ScriptObject>
	{
		[OnStartEvent]
		public void RunMyStepRoutine()
		{
			StepRoutine.Start(MyStepRoutine());
			FrameUpdateEvent.Instance.RaiseEvent();
			TestEvent.Instance.RaiseEvent();
		}

		public IEnumerator MyStepRoutine()
		{
			Console.WriteLine("Before frame update");
			yield return new WaitFor(typeof(FrameUpdateEvent)); // This and...
			Console.WriteLine("After frame update");
			yield return new WaitFor(TestEvent.Instance); // ...this are both valid options for Singleton Script Events!
			Console.WriteLine("After test event class");
			yield break;
		}

		[OnStartEvent]
		public void RunFrameUpdateStepRoutine()
		{
			StepRoutine.Start(FrameStepRoutine());
		}

		private IEnumerator FrameStepRoutine()
		{
			WaitFor waitForNewFrame = new WaitFor(typeof(FrameUpdateEvent));
			while (true)
			{
				yield return waitForNewFrame;
				Console.WriteLine("New frame drawn.");
			}
		}

		[OnStartEvent]
		public void RunPhysicsStepStepRoutine()
		{
			StepRoutine.Start(PhysicsStepRoutine());
		}

		private IEnumerator PhysicsStepRoutine()
		{
			WaitFor waitForNewPhysicsStep = new WaitFor(typeof(PhysicsTimeStepEvent));
			while (true)
			{
				yield return waitForNewPhysicsStep;
				Console.WriteLine("New physics step.");
			}
		}
	}
}
