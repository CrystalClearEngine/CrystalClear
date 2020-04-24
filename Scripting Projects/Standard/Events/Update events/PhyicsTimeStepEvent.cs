using CrystalClear.EventSystem;
using System.Threading;

namespace CrystalClear.Standard.Events
{
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
	public class PhysicsTimeStepEvent : UpdateScriptEvent<PhysicsTimeStepEvent>
	{

	}
}