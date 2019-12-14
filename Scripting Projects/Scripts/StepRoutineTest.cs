using CrystalClear.ScriptUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.HierarchySystem.Scripting;

namespace Scripts
{
	[IsScript]
	public class StepRoutineTest : HierarchyScript<ScriptObject>
	{
		[OnStartEvent]
		public void StepRoutineRunner()
		{
			Console.WriteLine("Trying to start routine!");
			StepRoutine.Start(MyStepRoutine());
			Console.WriteLine("Started routine!");

			FrameUpdateEventClass.Instance.RaiseEvent();
			FrameUpdateEventClass.Instance.RaiseEvent();
			FrameUpdateEventClass.Instance.RaiseEvent();
			FrameUpdateEventClass.Instance.RaiseEvent();
			TestEventClass.Instance.RaiseEvent();
			TestEventClass.Instance.RaiseEvent();
			TestEventClass.Instance.RaiseEvent();
			TestEventClass.Instance.RaiseEvent();
			TestEventClass.Instance.RaiseEvent();
		}

		public IEnumerator MyStepRoutine()
		{
			Console.WriteLine("Before frame update");
			yield return new WaitFor(typeof(FrameUpdateEventClass)); // This and...
			Console.WriteLine("After frame update");
			yield return new WaitFor(TestEventClass.Instance); // ...this are both valid options for Singleton Script Events!
			Console.WriteLine("After test event class");
			yield break;
		}
	}
}
