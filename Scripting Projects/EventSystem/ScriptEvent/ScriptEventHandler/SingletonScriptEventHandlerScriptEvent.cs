using System;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// A singleton version of the EventArgsScriptEvent. Contains implementation for a singleton.
	/// </summary>
	/// <typeparam name="InstanceType">The type of the instance. Should generally be the same as the deriving class.</typeparam>
	/// <typeparam name="TEventArgs">The event args type.</typeparam>
	public abstract class SingletonScriptEventHandlerScriptEvent<InstanceType> : ScriptEventHandlerScriptEvent
		where InstanceType : SingletonScriptEventHandlerScriptEvent<InstanceType>, new()
	{
		// Singleton stuff.

		// Static constructor to ensure that any SingletonScriptEvent will be created at runtime unless overriden in a deriving class.
		static SingletonScriptEventHandlerScriptEvent()
		{
		}

		// Protected parameterless constructor for new T() and deriving classes.
		protected SingletonScriptEventHandlerScriptEvent()
		{
		}

		// The instance.
		private static InstanceType _instance;

		// The instance's public property.
		public static InstanceType Instance => _instance ?? (_instance = new InstanceType());
	}
}
