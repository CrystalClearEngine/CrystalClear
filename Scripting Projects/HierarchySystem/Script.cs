using CrystalClear.EventSystem;
using CrystalClear.ScriptUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrystalClear.HierarchySystem.Scripting
{
	/// <summary>
	/// Stores the type and instance of a Script.
	/// </summary>
	public struct Script // TODO store a list of all events that this Script is subscribed to! We need to remove it's reference from there too to delete it... maybe make it a disposable aswell?
	{
		/// <summary>
		/// The instance of the Script.
		/// </summary>
		public readonly object ScriptInstance;

		/// <summary>
		/// The type of the Script.
		/// </summary>
		public Type ScriptType
		{
			get;
			private set;
		}

		/// <summary>
		/// Creates a Script of any type and initializes it as an HierarchyScript if necessary.
		/// </summary>
		/// <param name="scriptType">The type of the Script.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		/// <param name="attatchedTo">The HierarchyObject to attatch this Script to (provided it is a HierarchyScript!).</param>
		public Script(Type scriptType, object[] constructorParameters = null, HierarchyObject attatchedTo = null)
		{
			// Is scriptType a HierarchyScript?
			if (HierarchyScript.IsHierarchyScript(scriptType))
			{
				// Initialize this to new Script() for HierarchyObjects.
				this = new Script(attatchedTo, scriptType, constructorParameters);
			}
			else
			{
				// Initialize this to new Script() for any type.
				this = new Script(scriptType, constructorParameters);
			}
		}

		/// <summary>
		/// Creates a Script of any type.
		/// </summary>
		/// <param name="scriptType">The type of the Script.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public Script(Type scriptType, object[] constructorParameters = null)
		{
			// IsScript check.
			if (!IsScript(scriptType))
			{
				throw new ArgumentException("The provided type is not a script!");
			}

			// Assign ScriptType.
			ScriptType = scriptType;

			// Are there any constructor parameters?
			if (constructorParameters != null)
			{
				// Assign ScriptInstance to an instance of the Script using the provided constructor parameters.
				ScriptInstance = Activator.CreateInstance(scriptType, constructorParameters);
			}
			else
			{
				// Assign ScriptInstance to an instance of the Script.
				ScriptInstance = Activator.CreateInstance(scriptType);
			}

			// Subscribe the events.
			EventSystem.EventSystem.SubscribeEvents(scriptType, ScriptInstance);
		}

		/// <summary>
		/// Creates a HierarchyScript and instanciates it with the provided parameters.
		/// </summary>
		/// <param name="attatchedTo">The HierarchyObject that this script is attatched to.</param>
		/// <param name="scriptType">The type of the HierarchyObject.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public Script(HierarchyObject attatchedTo, Type scriptType, object[] constructorParameters = null) // TODO (maybe) use compiled lambdas and expressions for better performance! https://vagifabilov.wordpress.com/2010/04/02/dont-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions/
		{
			// IsScript check.
			if (!IsScript(scriptType))
			{
				throw new ArgumentException("The provided type is not a script!");
			}

			// Assign ScriptType.
			ScriptType = scriptType;

			// Assign ScriptInstance to the return of HierarchyScript.CreateHierarchyScript, which will be an instance of the script.
			ScriptInstance = HierarchyScript.CreateHierarchyScript(attatchedTo, scriptType, constructorParameters);

			// Subscribe events.
			EventSystem.EventSystem.SubscribeEvents(scriptType, ScriptInstance);
		}

		/// <summary>
		/// Finds all types with the script attribute and returns them.
		/// </summary>
		/// <param name="assembly">The assembly to find the scripts in.</param>
		/// <returns>The found scripts.</returns>
		public static Type[] FindScriptTypesInAssembly(Assembly assembly) => FindScriptTypesInTypes(assembly.GetTypes());

		/// <summary>
		/// Finds all types with the script attribute and returns them.
		/// </summary>
		/// <param name="types">The types to find the scripts in.</param>
		/// <returns>The found scripts.</returns>
		public static Type[] FindScriptTypesInTypes(params Type[] types)
		{
			// Find and store the found script types.
			Type[] scripts = (from type in types // Iterator variable.
							  where IsScript(type) // Is this a script?
							  select type).ToArray();
			// Return scripts.
			return scripts;
		}

		/// <summary>
		/// Calls a method in the script by method name.
		/// </summary>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="parameters">The paramaters for the call.</param>
		/// <returns>The return of the call (if any).</returns>
		public object DynamicallyCallMethod(string methodName, params object[] parameters)
		{
			// Initialize list for the found parameter type.
			List<Type> parameterTypes = new List<Type>();

			// Iterate through all provided parameters and add the type of the parameter to the list of parameter types.
			foreach (object parameter in parameters)
			{
				parameterTypes.Add(parameter.GetType());
			}

			// Are the parameterTypes not empty? That would mean we can use them to aid in our search.
			if (parameterTypes.Count > 0)
			{
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
				throw new ArgumentException("Unequal array sizes - array lengths of classesToSubscribe and instances dont match");
			}

			// Initialize list called returnObjects which is used to store all the returns of the objects.
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

			// Return returnObjects as an array because it is neater that way.
			return returnObjects.ToArray();
		}

		/// <summary>
		/// Checks wether or not a type is a Script (Non-static and has the IsScriptAttribute).
		/// </summary>
		/// <param name="toCheck">The type to check wether it is a Script.</param>
		/// <returns>Wether or not the type is a Script.</returns>
		public static bool IsScript(Type toCheck)
		{
			return toCheck.GetCustomAttribute<IsScriptAttribute>() != null // Does this type have the IsScriptAttribute attribute?
				&& !(toCheck.IsAbstract && toCheck.IsSealed); // Static check.
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