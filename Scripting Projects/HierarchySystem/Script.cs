using CrystalClear.ScriptUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrystalClear.HierarchySystem.Scripting
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
		/// <param name="scriptType">The script's type.</param>
		public Script(HierarchyObject attatchedTo, Type scriptType) // TODO (maybe) use compiled lambdas and expressions for better performance! https://vagifabilov.wordpress.com/2010/04/02/dont-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions/
		{
			// Assign ScriptType.
			ScriptType = scriptType;

			// Assign ScriptInstance to the return of HierarchyScript.CreateHierarchyScript, which will be an instance of the script.
			ScriptInstance = HierarchyScript.CreateHierarchyScript(attatchedTo, scriptType);

			// Subscribe the events.
			EventSystem.EventSystem.SubscribeEvents(ScriptType, ScriptInstance);
		}

		/// <summary>
		/// Finds all types with the script attribute and returns them.
		/// </summary>
		/// <param name="assembly">The assembly to find the scripts in.</param>
		/// <returns>The found scripts.</returns>
		public static Type[] FindScriptTypesInAssembly(Assembly assembly)
		{
			// Find and store the found script types.
			Type[] scripts = (from exportedType in assembly.GetTypes()
							  from attribute in exportedType.GetCustomAttributes()
							  where attribute is IsScriptAttribute
							  select exportedType).ToArray();
			return scripts; // Return scripts.
		}

		/// <summary>
		/// Finds all types with the script attribute and returns them.
		/// </summary>
		/// <param name="types">The types to find the scripts in.</param>
		/// <returns>The found scripts.</returns>
		public static Type[] FindScriptTypesInTypes(params Type[] types)
		{
			// Find and store the found script types.
			Type[] scripts = (from type in types
							  from attribute in type.GetCustomAttributes()
							  where attribute is IsScriptAttribute
							  select type).ToArray();
			return scripts; // Return scripts.
		}


		/// <summary>
		/// Calls a method in the script by method name.
		/// </summary>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="parameters">The paramaters for the call.</param>
		/// <returns>The return of the call (if any).</returns>
		public object DynamicallyCallMethod(string methodName, params object[] parameters)
		{
			List<Type> parameterTypes = new List<Type>();

			foreach (object parameter in parameters)
			{
				parameterTypes.Add(parameter.GetType());
			}

			// Are the parameterTypes not empty?
			if (parameterTypes.Count > 0)
			{ // That means we can use them to aid in our search.

				// Return the result of the invoke.
				return ScriptType.GetMethod(methodName, parameterTypes.ToArray()).Invoke(ScriptInstance, parameters);
			}

			// Return the result of the invoke.
			return ScriptType.GetMethod(methodName).Invoke(ScriptInstance, parameters);
		}

		/// <summary>
		/// Calls methods in the script by method name.
		/// </summary>
		/// <returns>The returns of the calls.</returns>
		public object[] DynamicallyCallMethods(string[] methodNames, object[][] parametersList = null)
		{
			// Bulk check.
			if (methodNames.Length == parametersList.Length)
			{ // Uh oh. We have recieved differently sized arrays...
				throw new Exception("Unequal array sizes - array lengths of classesToSubscribe and instances dont match");
			}

			// Initialize returnObjects.
			List<object> returnObjects = new List<object>();

			// Iterate through all names in methodNames.
			for (int i = 0; i < methodNames.Length; i++)
			{
				// Set the help string methodName to methodNames at i index.
				string methodName = methodNames[i];
				// Set the help object parameters to parameterList at i index.
				object[] parameters = parametersList[i];

				// Add return to returnObjects.
				returnObjects.Add(
					// Get the method named methodName.
					ScriptType.GetMethod(methodName)
					// Invoke the method.
					.Invoke(ScriptInstance, parameters));
			}

			// Return returnObjects as an array.
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