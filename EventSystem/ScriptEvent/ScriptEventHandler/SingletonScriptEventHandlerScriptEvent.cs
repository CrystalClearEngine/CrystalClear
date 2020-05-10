namespace CrystalClear.EventSystem
{
	/// <summary>
	/// A singleton version of the EventArgsScriptEvent. Contains implementation for a singleton.
	/// </summary>
	/// <typeparam name="TInstance">The type of the instance. Should generally be the same as the deriving class.</typeparam>
	public abstract class SingletonScriptEventHandlerScriptEvent<TInstance> : ScriptEventHandlerScriptEvent
		where TInstance : SingletonScriptEventHandlerScriptEvent<TInstance>, new()
	{
		// Singleton stuff.

		// Static constructor to ensure that SingletonScriptEvent can't be instanciated.
		static SingletonScriptEventHandlerScriptEvent()
		{
		}

		// Protected parameterless constructor for new T() and deriving classes.
		protected SingletonScriptEventHandlerScriptEvent()
		{
		}

		// The instance.
		private static TInstance _instance;

		// The instance's public property.
		public static TInstance Instance => _instance ?? (_instance = new TInstance());
	}
}
