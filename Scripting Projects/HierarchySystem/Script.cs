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
		/// <param name="scriptType">The script´s type.</param>
		public Script(HierarchyObject attatchedTo, Type scriptType) // TODO (maybe) use compiled lambdas and expressions for better performance! https://vagifabilov.wordpress.com/2010/04/02/dont-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions/
		{
			ScriptType = scriptType; // Assign ScriptType.

			ScriptInstance = HierarchyScript.CreateHierarchyScript(attatchedTo, scriptType);

			EventSystem.EventSystem.SubscribeEvents(ScriptType, ScriptInstance); // Subscribe the events.
		}

		/// <summary>
		/// Finds all types with the script attribute and returns them.
		/// </summary>
		/// <param name="assembly">The assembly to find the scripts in.</param>
		/// <returns>The found scripts.</returns>
		public static Type[] FindScriptTypesInAssembly(Assembly assembly)
		{
			Type[] scripts = (from exportedType in assembly.GetTypes()
								from attribute in exportedType.GetCustomAttributes()
								where attribute is IsScriptAttribute
								select exportedType).ToArray();
			return scripts;
		}

		/// <summary>
		/// Finds all types with the script attribute and returns them.
		/// </summary>
		/// <param name="types">The types to find the scripts in.</param>
		/// <returns>The found scripts.</returns>
		public static Type[] FindScriptTypesInTypes(Type[] types)
		{
			Type[] scripts = (from type in types
							  from attribute in type.GetCustomAttributes()
							  where attribute is IsScriptAttribute
							  select type).ToArray();
			return scripts;
		}


		/// <summary>
		/// Calls a method in the script by method name.
		/// </summary>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="parameters">The paramaters for the call.</param>
		/// <returns>The return of the invoke (if any).</returns>
		public object DynamicallyCallMethod(string methodName, object[] parameters = null, Type[] parameterTypes = null)
		{
			if (parameterTypes != null)
			{
				return ScriptType.GetMethod(methodName, parameterTypes).Invoke(ScriptInstance, parameters);
			}

			return ScriptType.GetMethod(methodName).Invoke(ScriptInstance, parameters);
		}

		/// <summary>
		/// Calls methods in the script by method name.
		/// </summary>
		/// <returns>The returns of the invokes.</returns>
		public object[] DynamicallyCallMethods(string[] methodNames, object[][] parametersList = null)
		{
			if (methodNames.Length == parametersList.Length)
			{
				throw new Exception("Incorrect array sizes - array lengths of classesToSubscribe and instances dont match");
			}

			List<object> returnObjects = new List<object>();
			MethodInfo[] methods = ScriptType.GetMethods();
			for (int i = 0; i < methodNames.Length; i++)
			{
				foreach (MethodInfo method in methods)
				{
					if (method.Name == methodNames[i])
					{
						returnObjects.Add(method.Invoke(ScriptInstance, parametersList[i]));
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