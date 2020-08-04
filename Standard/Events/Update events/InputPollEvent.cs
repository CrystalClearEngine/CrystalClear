using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	///     The frame update event attribute.
	/// </summary>
	public sealed class OnInputPollAttribute : SubscribeToAttribute
	{
		public OnInputPollAttribute() : base(typeof(InputPollEvent))
		{
		}
	}

	/// <summary>
	///     The frame update event class.
	/// </summary>
	public class InputPollEvent : UpdatingScriptEvent<InputPollEvent>
	{
		[OnStartEvent]
		private static void start()
		{
			Start();
		}

		[OnStopEvent]
		private static void stop()
		{
			Stop();
		}
	}
}