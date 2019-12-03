using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SubscribeToAttribute : Attribute
	{
		public ScriptEvent ScriptEvent;

		public SubscribeToAttribute(Type eventType)
		{
			ScriptEvent = (ScriptEvent)eventType.GetProperty("Instance").GetValue(null); // TODO: replace this reflection with proper code. if possible...
		}
	}
}