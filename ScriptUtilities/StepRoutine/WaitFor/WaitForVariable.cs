﻿using CrystalClear.EventSystem;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	public sealed class WaitForVariable : WaitFor
	{
		private readonly ScriptEventBase eventToCheckOn;
		private readonly object toCheck;

		private readonly object toWaitFor;
		private ScriptEventHandler checker;

		// TODO: use in instead of ref?
		// TODO: add option to use ReflectionEquals as the equality comparator.
		public WaitForVariable(object toCheck, object toWaitFor, ScriptEventBase checkOn)
		{
			this.toCheck = toCheck;
			this.toWaitFor = toWaitFor;
			eventToCheckOn = checkOn;
		}

		public override void Start(StepRoutineInfo stepRoutine)
		{
			/* To achieve the effect we want a delegate that checks whether toCheck equals toWaitFor
			 * and if it does proceed the StepRoutine using ProceedStepRoutine.
			 * Then we want to subscribe that delegate to the eventToCheckOn so it will be called when
			 * the event is raised.
			 */

			// Create the checking and proceeding delegate.
			checker = delegate
			{
				if (toCheck is null && toWaitFor is null) // Both are null? Proceed.
				{
					StepRoutine.ProceedStepRoutine(stepRoutine);
					return;
				}

				if (toCheck is null || toWaitFor is null) // Only one is null? Not there yet.
					return;

				if (toCheck.Equals(eventToCheckOn))
				{
					StepRoutine.ProceedStepRoutine(stepRoutine);
				}
			};

			// Subscribe the checking and proceeding delegate to the event so that it is called when
			// it is raised.
			eventToCheckOn.Subscribe(checker);
		}

		public override void Cancel()
		{
			eventToCheckOn.Unsubscribe(checker);
		}

		internal override void Cleanup()
		{
			eventToCheckOn.Unsubscribe(checker);
		}
	}
}