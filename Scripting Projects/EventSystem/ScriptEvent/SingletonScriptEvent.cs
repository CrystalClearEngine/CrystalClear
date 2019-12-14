using System;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// A singleton version of the EventArgsScriptEvent. Contains implementation for a singleton.
	/// </summary>
	/// <typeparam name="TInstance">The type of the instance. Should generally be the same as the deriving class.</typeparam>
	public abstract class SingletonScriptEvent<TInstance, TScriptEvent> : ScriptEvent
		where TInstance : SingletonScriptEventHandlerScriptEvent<TInstance>, new()
		where TScriptEvent : ScriptEvent, new()
	{
		// Singleton stuff.

		// Static constructor to ensure that SingletonScriptEvent can't be instanciated.
		static SingletonScriptEvent()
		{
		}

		// Protected parameterless constructor for new T() and deriving classes.
		protected SingletonScriptEvent()
		{
		}

		// The instance.
		private static TInstance _instance;

		// The instance's public property.
		public static TInstance Instance => _instance ?? (_instance = new TInstance());
	}
}
