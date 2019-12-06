using CrystalClear.EventSystem;
using System;

namespace CrystalClear.Standard.Events
{
	public class OnStartEventAttribute : SubscribeToAttribute
	{
		public OnStartEventAttribute() : base(typeof(StartEventClass))
		{
		}
	}

	public class StartEventClass : SingletonScriptEvent<StartEventClass, EventArgs>
	{
	}
}