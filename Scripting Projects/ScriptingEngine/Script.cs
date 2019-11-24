using CrystalClear.EventSystem;
using CrystalClear.ScriptUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrystalClear.ScriptingEngine
{
	/// <summary>
	/// Stores the type and (currently) instance of a script. This is really a helper class.
	/// </summary>
	public struct Script
	{
		/// <summary>
		/// The current instance of this script. This is going to be replaced in the future as the instance will be stored in the HierarchyObject that has the script!
		/// </summary>
		public object ScriptInstance;

		/// <summary>
		/// The type of the script class.
		/// </summary>
		public Type ScriptType;

		public Script(Type scriptClass)
		{
			ScriptType = scriptClass;
			ScriptInstance = Activator.CreateInstance(scriptClass);
		}

		/// <summary>
		/// Finds all classes with the script attribute and that inherits from HierarchyObject or a derivative thereof and returns them.
		/// </summary>
		/// <param name="assembly">The assembly to find the scripts in</param>
		/// <returns>The found scripts</returns>
		public static Script[] FindScriptsInAssembly(Assembly assembly)
		{
			Script[] scripts = (from exportedType in assembly.GetExportedTypes()
								from attribute in exportedType.GetCustomAttributes()
								where attribute is IsScriptAttribute
								select new Script(exportedType)).ToArray();
			return scripts;
		}

		/// <summary>
		/// Calls a method in the script by method name.
		/// </summary>
		/// <param name="methodName">The name of the method</param>
		/// <param name="parameters">The paramaters for the call</param>
		/// <returns>The return value (if any)</returns>
		public object DynamicallyCallMethod(string methodName, object[] parameters = null)
		{
			foreach (MethodInfo method in ScriptType.GetMethods())
			{
				if (method.Name == methodName)
				{
					return method.Invoke(ScriptInstance, parameters);
				}
			}

			throw new MethodNotFoundException();
		}

		/// <summary>
		/// Calls methods in the script by method name.
		/// </summary>
		/// <returns>The return values</returns>
		public object[] DynamicallyCallMethods(string[] methodNames, List<object[]> parametersList = null)
		{
			List<object> returnObjects = new List<object>();
			MethodInfo[] methods = ScriptType.GetMethods();
			for (int i = 0; i < methodNames.Length; i++)
			{
				foreach (MethodInfo method in methods)
				{
					if (method.Name == methodNames[i])
					{
						returnObjects.Add(method.Invoke(ScriptInstance, parametersList?[i]));
					}
				}
			}

			return returnObjects.ToArray();
		}

		/// <summary>
		/// Subscribes all events in the script.
		/// </summary>
		public void SubscribeAllEvents()
		{
			foreach (MethodInfo method in ScriptType.GetMethods())
			{
				foreach (Attribute attribute in method.GetCustomAttributes())
				{
					if (attribute is SubscribeToAttribute subscribeToAttribute)
					{
						subscribeToAttribute.Event.Subscribe(method, ScriptInstance);
					}
				}
			}
		}

		#region Exceptions
		public class ScriptException : Exception
		{
		}

		public class MethodNotFoundException : ScriptException
		{
		}
		#endregion
	}
}