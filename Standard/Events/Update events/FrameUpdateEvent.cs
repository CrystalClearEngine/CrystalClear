using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;
using System;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The frame update event attribute.
	/// </summary>
	public sealed class OnFrameUpdateAttribute : SubscribeToAttribute
	{
		public OnFrameUpdateAttribute() : base(typeof(FrameUpdateEvent))
		{
		}
	}

	/// <summary>
	/// The frame update event class.
	/// </summary>
	public class FrameUpdateEvent : UpdateScriptEvent<FrameUpdateEvent>
	{
		[OnStartEvent]
		private static void start() => Start(new TimeSpan());

		[OnStopEvent]
		private static void stop() => Stop();
	}

}