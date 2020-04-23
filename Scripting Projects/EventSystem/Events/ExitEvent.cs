using CrystalClear.EventSystem;
using System;

namespace CrystalClear.EventSystem.StandardEvents // TODO: should this be Standard.Events or just something else?
{
	/// <summary>
	/// The exit event attribute.
	/// </summary>
	public sealed class OnExitEvent : SubscribeToAttribute
	{
		public OnExitEvent() : base(typeof(StopEvent))
		{
		}
	}

	// TODO: improve these documentations, include for example when they are raised.
	/// <summary>
	/// The exit event class.
	/// </summary>
	public class StopEvent : SingletonScriptEventHandlerScriptEvent<StartEvent>
	{

	}
}