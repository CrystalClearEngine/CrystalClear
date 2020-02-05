﻿using CrystalClear.EventSystem;
using System.Threading;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The physics time step event attribute.
	/// </summary>
	public sealed class OnPhysicsTimeStep : SubscribeToAttribute
	{
		public OnPhysicsTimeStep() : base(typeof(FrameUpdateEvent))
		{
		}
	}

	/// <summary>
	/// The physics time step event class.
	/// </summary>
	public class PhysicsTimeStepEventClass : SingletonScriptEventHandlerScriptEvent<PhysicsTimeStepEventClass>
	{
		public static void PhysicsTimeStepLoop(int TimeStepLengthMS = 20)
		{
			while (true)
			{
				Thread.Sleep(TimeStepLengthMS);
				Instance.RaiseEvent();
			}
		}
	}
}