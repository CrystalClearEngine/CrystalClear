using System;
using System.Collections.Generic;
using System.Reflection;

namespace CrystalClear.Scripting.ScriptingEngine
{
	public class Script
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
	}
}