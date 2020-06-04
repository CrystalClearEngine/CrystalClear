namespace CrystalClear.EventSystem.StandardEvents // TODO: should this be Standard.Events or just something else?
{
	/// <summary>
	/// The stop event attribute.
	/// </summary>
	public sealed class OnStopEvent : SubscribeToAttribute
	{
		public OnStopEvent() : base(typeof(StopEvent))
		{
		}
	}

	// TODO: improve these documentations, include for example when they are raised and why.
	/// <summary>
	/// The stop event class.
	/// </summary>
	public class StopEvent : ScriptEvent<StopEvent>
	{

	}
}