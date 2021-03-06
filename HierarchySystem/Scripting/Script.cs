﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrystalClear.ScriptUtilities;
using CrystalClear.SerializationSystem.ImaginaryObjects;

namespace CrystalClear.HierarchySystem.Scripting
{
	// TODO: rename to ScriptInfo?
	/// <summary>
	///     Stores the type and instance of a Script.
	/// </summary>
	public sealed class Script
	{
		public readonly object ScriptInstance;

		public Script(ImaginaryObject scriptBase, HierarchyObject attachedTo = null)
		{
			if (!IsScript(((IGeneralImaginaryObject) scriptBase).TypeData.GetConstructionType()))
			{
				throw new ArgumentException("ScriptBase is not a Script!");
			}

			ScriptInstance = scriptBase.CreateInstance();

			ScriptType = ((IGeneralImaginaryObject) scriptBase).TypeData.GetConstructionType();

			if (ScriptType.IsHierarchyScript())
			{
				HierarchyScript.SetUp(ScriptInstance, attachedTo);
			}

			EventSystem.EventSystem.SubscribeEvents(ScriptType, ScriptInstance);
		}

		public Type ScriptType { get; }

		#region Script Management

		public void UnsubscribeAll()
		{
			EventSystem.EventSystem.UnsubscribeEvents(ScriptType, ScriptInstance);
		}

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
				throw new ArgumentException(
					"Unequal array sizes - array lengths of classesToSubscribe and instances are not equal.");
			}

			List<object> returnObjects = new List<object>();

			for (var i = 0; i < methodNames.Length; i++)
			{
				var methodName = methodNames[i];
				object[] parameters = parametersList[i];

				returnObjects.Add(ScriptType.GetMethod(methodName).Invoke(ScriptInstance, parameters));
			}

			return returnObjects.ToArray();
		}

		#endregion

		#region Script Utilities

		public static Type[] FindScriptTypesInAssemblies(Assembly[] assemblies)
		{
			List<Type> typesInAssemblies = new List<Type>();

			foreach (var assembly in assemblies)
			{
				typesInAssemblies.AddRange(assembly.GetTypes());
			}

			return FindScriptTypesInTypes(typesInAssemblies.ToArray());
		}

		/// <summary>
		///     Finds all types with the script attribute and returns them.
		/// </summary>
		/// <param name="assembly">The assembly to find the scripts in.</param>
		/// <returns>The found scripts.</returns>
		public static Type[] FindScriptTypesInAssembly(Assembly assembly) =>
			FindScriptTypesInTypes(assembly.GetTypes());

		/// <summary>
		///     Finds all types with the script attribute and returns them.
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
		///     Checks whether or not a type is a Script (Non-static and has the IsScriptAttribute).
		/// </summary>
		/// <param name="toCheck">The type to check whether it is a Script.</param>
		/// <returns>whether or not the type is a Script.</returns>
		public static bool IsScript(Type toCheck) =>
			toCheck.GetCustomAttribute<IsScriptAttribute>() !=
			null // Does this type have the IsScriptAttribute attribute?
			&& !(toCheck.IsAbstract && toCheck.IsSealed); // Static check.

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