using System;
using System.Data.Common;
using System.Reflection;

namespace CrystalClear.Scripting.ScriptingEngine
{
	public class Script
	{
		public Script(Type scriptClass)
		{
			ScriptType = scriptClass;
			ScriptInstance = Activator.CreateInstance(scriptClass);
		}

		/// <summary>
		/// The type of the script class
		/// </summary>
		public Type ScriptType;

		public object ScriptInstance;

		public void DynamicallyCallMethod(string methodName, object[] parameters = null)
		{
			foreach (MethodInfo method in ScriptType.GetMethods())
			{
				if (method.Name == methodName)
				{
					method.Invoke(ScriptInstance, parameters);
				}
			}
		}
	}
}
