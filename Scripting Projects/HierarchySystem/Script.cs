using CrystalClear.EventSystem;
using CrystalClear.ScriptUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// Stores the type and instance of a HierarchyScript.
	/// </summary>
	public struct Script
	{
		/// <summary>
		/// The current instance of this script.
		/// </summary>
		public readonly object ScriptInstance;

		/// <summary>
		/// The type of the script class.
		/// </summary>
		public readonly Type ScriptType;

		/// <summary>
		/// Creates a script and instanciates it with the provided parameters.
		/// </summary>
		/// <param name="attatchedTo">The object that this script is attatched to.</param>
		/// <param name="scriptClass">The script´s type</param>
		/// <param name="parameters">The parameters to use for the constructor.</param>>
		public Script(HierarchyObject attatchedTo, Type scriptClass) // TODO (maybe) use compiled lambdas and expressions for better performance! https://vagifabilov.wordpress.com/2010/04/02/dont-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions/
		{
			ScriptType = scriptClass;

			ScriptInstance = ScriptType.BaseType.GetMethod("CreateHierarchyScript").MakeGenericMethod(attatchedTo.GetType()).Invoke(null, new object[] {attatchedTo, scriptClass});

			EventSystem.EventSystem.SubscribeEvents(ScriptType, ScriptInstance);
		}

		/// <summary>
		/// Finds all classes with the script attribute and that inherits from HierarchyObject or a derivative thereof and returns them.
		/// </summary>
		/// <param name="assembly">The assembly to find the scripts in.</param>
		/// <returns>The found scripts.</returns>
		public static Type[] FindScriptTypesInAssembly(Assembly assembly)
		{
			Type[] scripts = (from exportedType in assembly.GetExportedTypes()
								from attribute in exportedType.GetCustomAttributes()
								where attribute is IsScriptAttribute
								select exportedType).ToArray();
			return scripts;
		}

		/// <summary>
		/// Calls a method in the script by method name.
		/// </summary>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="parameters">The paramaters for the call.</param>
		/// <returns>The return value (if any).</returns>
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
		/// <returns>The return values.</returns>
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