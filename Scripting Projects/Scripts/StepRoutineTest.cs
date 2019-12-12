using CrystalClear.ScriptUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;

namespace Scripts
{
	[IsScript]
	public class StepRoutineTest : ScriptObject
	{
		[OnStartEvent]
		public void StepRoutineRunner()
		{
			Console.WriteLine("Trying to start routine!");
			StepRoutine.Start(MyStepRoutine());
			Console.WriteLine("Started routine!");
			FrameUpdateEventClass.Instance.RaiseEvent();
		}

		public IEnumerable MyStepRoutine()
		{
			Console.WriteLine("Before frame update");
			yield return new WaitFor(typeof(FrameUpdateEventClass));
			Console.WriteLine("After frame update");
		}
	}
}
