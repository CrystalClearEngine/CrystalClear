using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;
using System;
using System.Diagnostics;
using System.Threading;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The frame update event attribute.
	/// </summary>
	public sealed class OnInputPollAttribute : SubscribeToAttribute
	{
		public OnInputPollAttribute() : base(typeof(InputPollEvent))
		{
		}
	}

	/// <summary>
	/// The frame update event class.
	/// </summary>
	public class InputPollEvent : UpdateScriptEvent<InputPollEvent>
	{
		[OnStartEvent]
		private static void start() => Start(new TimeSpan());

		[OnStopEvent]
		private static void stop() => Stop();
	}
}