using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrystalClear.EventSystem;
using CrystalClear.ScriptUtils;

namespace CrystalClear.ScriptingEngine
{
	internal struct Script
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

		public object DynamicallyCallMethod(string methodName, object[] parameters = null)
		{
			foreach (MethodInfo method in ScriptType.GetMethods())
				if (method.Name == methodName)
					return method.Invoke(ScriptInstance, parameters);

			throw new Exception("Method not found!"); // Todo make into proper exception
		}

		public object[] DynamicallyCallMethods(string[] methodNames, List<object[]> parametersList = null)
		{
			List<object> returnObjects = new List<object>();
			MethodInfo[] methods = ScriptType.GetMethods();
			for (int i = 0; i < methodNames.Length; i++)
				foreach (MethodInfo method in methods)
					if (method.Name == methodNames[i])
						returnObjects.Add(method.Invoke(ScriptInstance, parametersList?[i]));

			return returnObjects.ToArray();
		}

		public static Script[] FindScripts(Assembly assembly)
		{
			Script[] scripts = (from exportedType in assembly.GetExportedTypes()
				from attribute in exportedType.GetCustomAttributes()
				where attribute is IsScriptAttribute
				select new Script(exportedType)).ToArray();
			return scripts;
		}

		public void SubscribeAllEvents()
		{
			foreach (MethodInfo method in ScriptType.GetMethods())
			foreach (Attribute attribute in method.GetCustomAttributes())
				if (attribute is SubscribeToAttribute subscribeToAttribute)
				{
					//if (Delegate.CreateDelegate(subscribeToAttribute.EventType, ScriptInstance, method) is
					//	StartEventHandler startEventHandler)
					//	StartEventClass.StartEvent += startEventHandler;
					//if (Delegate.CreateDelegate(subscribeToAttribute.EventType, ScriptInstance, method) is
					//	ExitEventHandler exitEventHandler)
					//	ExitEventClass.ExitEvent += exitEventHandler;
				}
		}
	}
}