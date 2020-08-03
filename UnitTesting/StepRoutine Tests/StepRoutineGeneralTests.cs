using CrystalClear.ScriptUtilities.StepRoutines;
using CrystalClear.Standard.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace UnitTests
{
	[TestClass]
	public class StepRoutineGeneralTests
	{
		public bool SimpleStepRoutineWaitDone = false;
		public bool GenericSimpleStepRoutineWaitDone = false;

		[TestMethod]
		public void SimpleStepRoutineStartTest()
		{
			StepRoutineInfo simpleStepRoutineInfo = SimpleStepRoutine().StartStepRoutine("SimpleStepRoutine");
			StepRoutineInfo genericSimpleStepRoutineInfo = GenericSimpleStepRoutine().StartStepRoutine("GenericSimpleStepRoutine");

			Assert.IsFalse(SimpleStepRoutineWaitDone);
			Assert.IsFalse(GenericSimpleStepRoutineWaitDone);

			TestEvent.Instance.RaiseEvent();

			Assert.IsTrue(SimpleStepRoutineWaitDone);
			Assert.IsTrue(GenericSimpleStepRoutineWaitDone);

			Assert.IsTrue(ReferenceEquals(simpleStepRoutineInfo, StepRoutineManager.GetStepRoutine(simpleStepRoutineInfo.StepRoutineId)));
			Assert.IsTrue(ReferenceEquals(genericSimpleStepRoutineInfo, StepRoutineManager.GetStepRoutine(genericSimpleStepRoutineInfo.StepRoutineId)));

			Assert.IsTrue(ReferenceEquals(simpleStepRoutineInfo, StepRoutineManager.GetStepRoutine(simpleStepRoutineInfo.StepRoutineName)));
			Assert.IsTrue(ReferenceEquals(genericSimpleStepRoutineInfo, StepRoutineManager.GetStepRoutine(genericSimpleStepRoutineInfo.StepRoutineName)));
		}

		public IEnumerator SimpleStepRoutine()
		{
			yield return new WaitForEvent(typeof(TestEvent));
			SimpleStepRoutineWaitDone = true;
		}

		public IEnumerator<WaitFor> GenericSimpleStepRoutine()
		{
			yield return new WaitForEvent(typeof(TestEvent));
			GenericSimpleStepRoutineWaitDone = true;
		}
	}
}
