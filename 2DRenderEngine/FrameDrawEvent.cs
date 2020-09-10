using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;

namespace CrystalClear.RenderEngine2D
{
	/// <summary>
	///     The frame draw event attribute.
	/// </summary>
	public sealed class OnFrameDrawAttribute : SubscribeToAttribute
	{
		public OnFrameDrawAttribute() : base(typeof(FrameDrawEvent))
		{
		}
	}

	/// <summary>
	///     The frame draw event class.
	/// </summary>
	public class FrameDrawEvent : ScriptEvent<FrameDrawEvent>
	{
	}
}