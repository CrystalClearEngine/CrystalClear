using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SubscribeToAttribute : Attribute
	{
		public ScriptEvent ScriptEvent;
		public Type EventType;

		public SubscribeToAttribute(Type eventType)
		{
			EventType = eventType;

			ScriptEvent = 
				(ScriptEvent)eventType
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null); // TODO: replace this reflection with "proper" code. if possible...
		}
	}
}