using System;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// A singleton version of the EventArgsScriptEvent. Contains implementation for a singleton.
	/// </summary>
	/// <typeparam name="InstanceType">The type of the instance. Should generally be the same as the deriving class.</typeparam>
	/// <typeparam name="TEventArgs">The event args type.</typeparam>
	public abstract class SingletonEventArgsScriptEvent<InstanceType, TEventArgs> : EventArgsScriptEvent<TEventArgs>
		where InstanceType : SingletonEventArgsScriptEvent<InstanceType, TEventArgs>, new()
		where TEventArgs : EventArgs
	{
		// Singleton stuff.

		// Static constructor to ensure that any SingletonScriptEvent will be created at runtime unless overriden in a deriving class.
		static SingletonEventArgsScriptEvent()
		{
		}

		// Protected constructor for new T() and deriving classes.
		protected SingletonEventArgsScriptEvent()
		{
		}

		// The instance.
		private static InstanceType _instance;

		// The instance's public property.
		public static InstanceType Instance => _instance ?? (_instance = new InstanceType());
	}
}
