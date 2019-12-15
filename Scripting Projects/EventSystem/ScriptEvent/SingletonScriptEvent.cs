using System;

namespace CrystalClear.EventSystem
{
	/// <summary>
	/// A singleton version for ScriptEvent. Contains implementation for a singleton.
	/// </summary>
	/// <typeparam name="TInstance">The type of the instance. Should generally be the same as the deriving class.</typeparam>
	public abstract class SingletonScriptEvent<TScriptEvent> : ScriptEvent
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
		private static TScriptEvent _instance;

		// The instance's public property.
		public static TScriptEvent Instance => _instance ?? (_instance = new TScriptEvent());
	}
}
