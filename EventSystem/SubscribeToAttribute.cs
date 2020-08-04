using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SubscribeToAttribute : Attribute
	{
		public Type EventType;

		// TODO: make this work globally.
		/// <summary>
		///     The position in the order to subscribe the method in. Works within the type only.
		/// </summary>
		public int Order = 0;

		public ScriptEventBase ScriptEvent;

		public SubscribeToAttribute(Type eventType)
		{
			EventType = eventType;

			ScriptEvent =
				(ScriptEventBase) eventType
					.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
					.GetValue(null); // TODO: replace this reflection with "proper" code. if possible...
		}
	}
}