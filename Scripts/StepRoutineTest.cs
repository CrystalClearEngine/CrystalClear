using System.Collections;
using CrystalClear;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.ScriptUtilities;
using CrystalClear.ScriptUtilities.StepRoutines;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;

namespace Scripts
{
	[IsScript]
	public class StepRoutineTest : HierarchyScript<ScriptObject>
	{
		[OnStartEvent]
		public void RunMyStepRoutine()
		{
			MyStepRoutine().StartStepRoutine();
			FrameUpdateEvent.Instance.RaiseEvent();
			TestEvent.Instance.RaiseEvent();
		}

		public IEnumerator MyStepRoutine()
		{
			Output.Log("Before frame update");
			yield return new WaitForEvent(typeof(FrameUpdateEvent)); // This and...
			Output.Log("After frame update");
			yield return
				new WaitForEvent(TestEvent.Instance); // ...this are both valid options for Singleton Script Events!
			Output.Log("After test event class");
		}

		[OnStartEvent]
		public void RunFrameUpdateStepRoutine()
		{
			FrameStepRoutine().StartStepRoutine();
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
			PhysicsStepRoutine().StartStepRoutine();
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