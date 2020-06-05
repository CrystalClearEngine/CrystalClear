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
		/// Creates a Script of any type and initializes it as an HierarchyScript if necessary.
		/// </summary>
		/// <param name="scriptType">The type of the Script.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		/// <param name="attatchedTo">The HierarchyObject to attatch this Script to (provided it is a HierarchyScript!).</param>
		public Script(Type scriptType, object[] constructorParameters = null, HierarchyObject attatchedTo = null)
		{
			if (HierarchyScript.IsHierarchyScript(scriptType))
			{
				// This constructor is used for creating HierarchyScripts.
				this = new Script(attatchedTo, scriptType, constructorParameters);
			}
			else
			{
				// This constructor is used for all other types of scripts.
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
			if (!IsScript(scriptType))
			{
				throw new ArgumentException($"The provided type is not a script! Type = {scriptType}");
			}

			ScriptType = scriptType;

			if (constructorParameters is null)
			{
				ScriptInstance = Activator.CreateInstance(scriptType);
			}
			else
			{
				ScriptInstance = Activator.CreateInstance(scriptType, constructorParameters);
			}

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
			if (!IsScript(scriptType))
			{
				throw new ArgumentException("The provided type is not a script!");
			}

			ScriptType = scriptType;

			ScriptInstance = HierarchyScript.CreateHierarchyScript(attatchedTo, scriptType, constructorParameters);

			EventSystem.EventSystem.SubscribeEvents(scriptType, ScriptInstance);
		}

		public readonly object ScriptInstance;

		public Type ScriptType { get; }

		public object DynamicallyCallMethod(string methodName, params object[] parameters)
		{
			List<Type> parameterTypes = new List<Type>();
			
			foreach (object parameter in parameters)
			{
				parameterTypes.Add(parameter.GetType());
			}

			if (parameterTypes.Count > 0)
			{
				return ScriptType.GetMethod(methodName, parameterTypes.ToArray()).Invoke(ScriptInstance, parameters);
			}

			return ScriptType.GetMethod(methodName).Invoke(ScriptInstance, parameters);
		}

		public object[] DynamicallyCallMethods(string[] methodNames, object[][] parametersList = null)
		{
			if (methodNames.Length != parametersList.Length)
			{
				throw new ArgumentException("Unequal array sizes - array lengths of classesToSubscribe and instances are not equal.");
			}

			List<object> returnObjects = new List<object>();

			for (int i = 0; i < methodNames.Length; i++)
			{
				string methodName = methodNames[i];
				object[] parameters = parametersList[i];

				returnObjects.Add(ScriptType.GetMethod(methodName).Invoke(ScriptInstance, parameters));
			}

			return returnObjects.ToArray();
		}

		#region Script Utilities
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
		/// Checks whether or not a type is a Script (Non-static and has the IsScriptAttribute).
		/// </summary>
		/// <param name="toCheck">The type to check whether it is a Script.</param>
		/// <returns>whether or not the type is a Script.</returns>
		public static bool IsScript(Type toCheck)
		{
			return toCheck.GetCustomAttribute<IsScriptAttribute>() != null // Does this type have the IsScriptAttribute attribute?
				&& !(toCheck.IsAbstract && toCheck.IsSealed); // Static check.
		}
		#endregion

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