using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SubscribeToAttribute : Attribute
	{
		public ScriptEventBase ScriptEvent;
		public Type EventType;

		// TODO: make this work globally.
		/// <summary>
		/// The position in the order to subscribe the method in. Works within the type only.
		/// </summary>
		public int Order = 0;

		public SubscribeToAttribute(Type eventType)
		{
			EventType = eventType;

			ScriptEvent =
				(ScriptEventBase)eventType
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null); // TODO: replace this reflection with "proper" code. if possible...
		}
	}
}