using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using CrystalClear.Scripting.EventSystem;
using CrystalClear.Scripting.EventSystem.Events;
using CrystalClear.Scripting.ScriptAttributes;

namespace CrystalClear.Scripting.ScriptingEngine
{
	public struct Script
	{
		public object ScriptInstance;

		/// <summary>
		///     The type of the script class
		/// </summary>
		public Type ScriptType;

		public Script(Type scriptClass)
		{
			ScriptType = scriptClass;
			ScriptInstance = Activator.CreateInstance(scriptClass);
		}

		public void DynamicallyCallMethod(string methodName, object[] parameters = null)
		{
			foreach (MethodInfo method in ScriptType.GetMethods())
				if (method.Name == methodName)
					method.Invoke(ScriptInstance, parameters);
		}

		public void DynamicallyCallMethods(string[] methodNames, List<object[]> parametersList = null)
		{
			var methods = ScriptType.GetMethods();
			for (var i = 0; i < methodNames.Length; i++)
				foreach (MethodInfo method in methods)
					if (method.Name == methodNames[i])
						method.Invoke(ScriptInstance, parametersList?[i]);
		}

		public static Script[] FindScripts(Assembly assembly)
		{
			Script[] scripts = (from exportedType in assembly.GetExportedTypes()
				from attribute in exportedType.GetCustomAttributes()
				where attribute is ScriptAttribute
				select new Script(exportedType)).ToArray();
			return scripts;
		}

		public List<Event> GetEvents()
		{
			List<Event> eventsList = new List<Event>();

			foreach (MethodInfo method in ScriptType.GetMethods())
				foreach (Attribute attribute in method.GetCustomAttributes())
					if (attribute is SubscribeToAttribute subscribeToAttribute)
					{
						eventsList.Add(new Event(subscribeToAttribute.EventType, method));
						//if (Delegate.CreateDelegate(subscribeToAttribute.EventType, ScriptInstance, method) is
						//	StartEventHandler startEventHandler)
						//	StartEventClass.StartEvent += startEventHandler;
						//if (Delegate.CreateDelegate(subscribeToAttribute.EventType, ScriptInstance, method) is
						//	ExitEventHandler exitEventHandler)
						//	ExitEventClass.ExitEvent += exitEventHandler;
					}

			return eventsList;
		}
	}
}