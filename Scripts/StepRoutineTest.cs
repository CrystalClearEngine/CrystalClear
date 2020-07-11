using CrystalClear;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.ScriptUtilities;
using CrystalClear.ScriptUtilities.StepRoutines;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
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
			Output.Log("Before frame update");
			yield return new WaitForEvent(typeof(FrameUpdateEvent)); // This and...
			Output.Log("After frame update");
			yield return new WaitForEvent(TestEvent.Instance); // ...this are both valid options for Singleton Script Events!
			Output.Log("After test event class");
			yield break;
		}

		[OnStartEvent]
		public void RunFrameUpdateStepRoutine()
		{
			StepRoutine.Start(FrameStepRoutine());
		}

		private IEnumerator FrameStepRoutine()
		{
			WaitFor waitForNewFrame = new WaitForEvent(typeof(FrameUpdateEvent));
			while (true)
			{
				yield return waitForNewFrame;
				Output.Log("New frame drawn.");
			}
		}

		[OnStartEvent]
		public void RunPhysicsStepStepRoutine()
		{
			StepRoutine.Start(PhysicsStepRoutine());
		}

		private IEnumerator PhysicsStepRoutine()
		{
			WaitFor waitForNewPhysicsStep = new WaitForEvent(typeof(PhysicsTimeStepEvent));
			while (true)
			{
				yield return waitForNewPhysicsStep;
				Output.Log("New physics step.");
			}
		}
	}
}
