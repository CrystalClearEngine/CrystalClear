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
	public struct Script
	{
		/// <summary>
		/// The instance of the Script.
		/// </summary>
		public readonly object ScriptInstance;

		/// <summary>
		/// The type of the Script.
		/// </summary>
		public readonly Type ScriptType;

		/// <summary>
		/// Creates a Script of any type and initializes it as an HierarchyScript if necessary.
		/// </summary>
		/// <param name="scriptType">The type of the Script.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		/// <param name="attatchedTo">The HierarchyObject to attatch this Script to (provided it is a HierarchyScript!).</param>
		public Script(Type scriptType, object[] constructorParameters = null, HierarchyObject attatchedTo = null)
		{
			// Is scriptType an HierarchyScript?
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
			// Assign ScriptType.
			ScriptType = scriptType;

			// Is the Script type not static?
			if (!scriptType.IsAbstract && !scriptType.IsSealed)
			{
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
			}
			else
				// There is no instance of ScriptType since it is static.
				ScriptInstance = null;

			// Subscribe the events.
			EventSystem.EventSystem.SubscribeEvents(ScriptType, ScriptInstance);
		}

		/// <summary>
		/// Creates a HierarchyScript and instanciates it with the provided parameters.
		/// </summary>
		/// <param name="attatchedTo">The HierarchyObject that this script is attatched to.</param>
		/// <param name="scriptType">The type of the HierarchyObject.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public Script(HierarchyObject attatchedTo, Type scriptType, object[] constructorParameters = null) // TODO (maybe) use compiled lambdas and expressions for better performance! https://vagifabilov.wordpress.com/2010/04/02/dont-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions/
		{
			// Assign ScriptType.
			ScriptType = scriptType;

			// Assign ScriptInstance to the return of HierarchyScript.CreateHierarchyScript, which will be an instance of the script.
			ScriptInstance = HierarchyScript.CreateHierarchyScript(attatchedTo, scriptType, constructorParameters);

			// Subscribe events.
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
			// Return scripts.
			return scripts;
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

		#region Exceptions
		public class ScriptException : Exception
		{
		}

		public class MethodNotFoundException : ScriptException
		{
		}
		#endregion
	}

	[Serializable]
	public class ScriptStorage
	{
		private readonly string assemblyQualifiedName;

		private readonly object[] constructorParameters;

		private readonly HierarchyObject attatchedTo;

		public ScriptStorage(Type scriptType, object[] constructorParameters = null, HierarchyObject attatchedTo = null)
		{
			assemblyQualifiedName = scriptType.AssemblyQualifiedName;
			this.constructorParameters = constructorParameters;
			this.attatchedTo = attatchedTo;
		}

		public Script CreateScript()
		{
			try
			{
				Script script = new Script(Type.GetType(assemblyQualifiedName), constructorParameters, attatchedTo); //TODO look up wether or not declaring a variable like this wastes any memory or if it is optimized. This does look cleaner than just returning I think, so for now it stays here and everywhere else!
				return script;
			}
			catch (TypeLoadException e)
			{
				throw new Exception($"The specified Script type can not be found. Make sure it is loaded correctly and that the type still exists. FullTypeName = {assemblyQualifiedName}. Full error message = {e}");
			}
		}
	}
}