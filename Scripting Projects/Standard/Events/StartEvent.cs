using CrystalClear.EventSystem;
using System;
using System.Collections.Generic;
using System.Reflection;

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