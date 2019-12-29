using CrystalClear.EventSystem;
using System;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The start event attribute.
	/// </summary>
	public class OnStartEventAttribute : SubscribeToAttribute
	{
		public OnStartEventAttribute() : base(typeof(StartEvent))
		{
		}
	}

	/// <summary>
	/// The start event class.
	/// </summary>
	public class StartEvent : SingletonScriptEventHandlerScriptEvent<StartEvent>
	{
		// Showing how methods can be overriden in deriving events.
		public override void RaiseEvent()
		{
			base.RaiseEvent();
			Console.WriteLine("The start event was raised successfully.");
		}
	}
}