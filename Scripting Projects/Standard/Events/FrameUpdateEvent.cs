using CrystalClear.EventSystem;
using System;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The frame update event attribute.
	/// </summary>
	public class OnFrameUpdateAttribute : SubscribeToAttribute
	{
		public OnFrameUpdateAttribute() : base(typeof(FrameUpdateEventClass))
		{
		}
	}

	/// <summary>
	/// The frame update event class.
	/// </summary>
	public class FrameUpdateEventClass : SingletonScriptEventHandlerScriptEvent<FrameUpdateEventClass>
	{
	}
}