﻿using CrystalClear.EventSystem;
using System;
using System.Reflection;

namespace CrystalClear.ScriptUtilities.StepRoutines
{
	/// <summary>
	/// Waits for an event to be raised, then proceeds the StepRoutine.
	/// </summary>
	public sealed class WaitForEvent : WaitFor
	{
		ScriptEventBase EventToWaitFor;

		Action proceeder; // A delegate that will simply call ProceedStepRoutine. It is here so that Cancel and Cleanup can access it.

		public WaitForEvent(Type scriptEventType)
		{
			EventToWaitFor =
				(ScriptEventBase)scriptEventType // TODO: make this into a utility method somewhere.
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null); // TODO: add an ISingleton<maybe T> that we can then do GetInstance from instead of this.
		}

		public WaitForEvent(ScriptEventBase scriptEvent)
		{
			EventToWaitFor = scriptEvent;
		}

		public override void Start(StepRoutineInfo stepRoutine)
		{
			/* To achieve the effect we want, we should create a delegate that calls ProceedStepRoutine.
			 * We subscribe that delegate to the event.
			 * When the event is raised ProceedStepRoutine will be called, then Cleanup will be called
			 * from there and the StepRoutine will proceed.
			*/

			// Create the proceeder delegate.
			proceeder = new Action( // TODO: make utility method that generates this exact delegate, or perhaps even have it as a property on WaitFor?
				delegate
				{
					StepRoutine.ProceedStepRoutine(stepRoutine);
				});

			// Subscribe the delegate to the event so it will be called when the event is raised.
			EventToWaitFor.Subscribe(proceeder);
		}

		public override void Cancel()
		{
			throw new NotImplementedException();
		}

		internal override void Cleanup()
		{
			throw new NotImplementedException();
		}
	}
}