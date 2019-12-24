﻿using CrystalClear.ScriptUtilities;
using System;
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
		public void RunMyStepRoutine()
		{
			StepRoutine.Start(MyStepRoutine());
			FrameUpdateEventClass.Instance.RaiseEvent();
			TestEventClass.Instance.RaiseEvent();
		}

		[OnStartEvent]
		public void RunFrameStepRoutine()
		{
			StepRoutine.Start(FrameStepRoutine());
		}

		private IEnumerator FrameStepRoutine()
		{
			WaitFor waitForNewFrame = new WaitFor(typeof(FrameUpdateEventClass));
			while(true)
			{
				yield return waitForNewFrame;
				Console.WriteLine("New frame drawn.");
			}
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
