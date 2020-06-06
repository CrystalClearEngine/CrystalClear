using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;
using System;

namespace CrystalClear.Standard.Events
{
	// TODO: should this instead be called OnPhysicsTimestep (the s not capitalized)?
	/// <summary>
	/// The physics time step event attribute.
	/// </summary>
	public sealed class OnPhysicsTimeStep : SubscribeToAttribute
	{
		public OnPhysicsTimeStep() : base(typeof(PhysicsTimeStepEvent))
		{
		}
	}

	/// <summary>
	/// The physics time step event class.
	/// </summary>
	public class PhysicsTimeStepEvent : UpdatingScriptEvent<PhysicsTimeStepEvent>
	{
		[OnStartEvent]
		private static void start() => Start(TimeSpan.FromMilliseconds(33.33));

		[OnStopEvent]
		private static void stop() => Stop();
	}
}