using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CrystalClear.ScriptUtilities.StepRoutines;

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
			var simpleStepRoutineInfo = SimpleStepRoutine().Start("SimpleStepRoutine");
			var genericSimpleStepRoutineInfo = GenericSimpleStepRoutine().Start("GenericSimpleStepRoutine");

			Assert.IsFalse(SimpleStepRoutineWaitDone);
			Assert.IsFalse(GenericSimpleStepRoutineWaitDone);

			TestEvent.Instance.RaiseEvent();

			Assert.IsTrue(SimpleStepRoutineWaitDone);
			Assert.IsTrue(GenericSimpleStepRoutineWaitDone);

			Assert.IsTrue(ReferenceEquals(simpleStepRoutineInfo, StepRoutineManager.GetStepRoutine(simpleStepRoutineInfo.StepRoutineId)));
			Assert.IsTrue(ReferenceEquals(genericSimpleStepRoutineInfo, StepRoutineManager.GetStepRoutine(genericSimpleStepRoutineInfo.StepRoutineId)));
		}

		public IEnumerator SimpleStepRoutine()
		{
			yield return new WaitFor(typeof(TestEvent));
			SimpleStepRoutineWaitDone = true;
		}

		public IEnumerator<WaitFor> GenericSimpleStepRoutine()
		{
			yield return new WaitFor(typeof(TestEvent));
			GenericSimpleStepRoutineWaitDone = true;
		}
	}
}
