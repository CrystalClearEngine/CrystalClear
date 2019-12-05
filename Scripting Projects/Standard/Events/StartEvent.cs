using CrystalClear.EventSystem;

namespace CrystalClear.Standard.Events
{
	public class OnStartEventAttribute : SubscribeToAttribute
	{
		public OnStartEventAttribute() : base(typeof(StartEventClass))
		{
		}
	}

	public class StartEventClass : StandardSingletonScriptEvent<StartEventClass>
	{
	}
}