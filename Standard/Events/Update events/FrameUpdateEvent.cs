using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;

// TODO: should these be in RuntimeMain instead?
namespace CrystalClear.Standard.Events
{
	/// <summary>
	///     The frame update event attribute.
	/// </summary>
	public sealed class OnFrameUpdateAttribute : SubscribeToAttribute
	{
		public OnFrameUpdateAttribute() : base(typeof(FrameUpdateEvent))
		{
		}
	}

	/// <summary>
	///     The frame update event class.
	/// </summary>
	public class FrameUpdateEvent : UpdatingScriptEvent<FrameUpdateEvent>
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