namespace CrystalClear.EventSystem.StandardEvents // TODO: should this be Standard.Events or just something else?
{
	/// <summary>
	/// The start event attribute.
	/// </summary>
	public sealed class OnStartEventAttribute : SubscribeToAttribute
	{
		public OnStartEventAttribute() : base(typeof(StartEvent))
		{
		}
	}

	/// <summary>
	/// The start event class.
	/// </summary>
	public class StartEvent : ScriptEvent<StartEvent>
	{
		// Showing how methods can be overriden in deriving events.
		public override void RaiseEvent()
		{
			base.RaiseEvent();
			Output.Log("The start event was raised successfuly.");
		}
	}
}