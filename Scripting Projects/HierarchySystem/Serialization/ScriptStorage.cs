﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace CrystalClear.HierarchySystem.Scripting
{
	/// <summary>
	/// ScriptStorage is a type specifically for allowing serialization and deserialization of Scripts.
	/// </summary>
	[DataContract(Name = "Script")] // For the DataContractSerializer...
	[Serializable] // For the BinaryFormatter...
	public class ScriptStorage
	{
		public ScriptStorage(Type scriptType, object[] constructorParameters = null, HierarchyObject attatchedTo = null)
		{
			this.assemblyQualifiedTypeName = scriptType.AssemblyQualifiedName;
			this.constructorParameters = constructorParameters;
			this.attatchedToPath = attatchedTo.Path;
		}

		#region Data
		/// <summary>
		/// The assemblyQualifiedName of the type of the Script.
		/// </summary>
		[DataMember(Name = "ScriptTypeName")]
		private readonly string assemblyQualifiedTypeName;

		/// <summary>
		/// The parameters to use when constructing this Script.
		/// </summary>
		[DataMember(Name = "ConstructorParameters")]
		private readonly object[] constructorParameters;

		/// <summary>
		/// The path to follow from HierarchyManager to find the HierarchyObject this Script is attatched to.
		/// </summary>
		[DataMember(Name = "AttatchedToPath")]
		private readonly string attatchedToPath; // TODO/REM Shouldn't HierarchyObjects have the job of attatching the script? Regardless this should still stay since others could get use out of it.
		#endregion

		#region Properties
		public Type Type
		{
			get
			{
				try
				{
					return Type.GetType(assemblyQualifiedTypeName);
				}
				catch (TypeLoadException e)
				{
					throw new Exception($"The specified Script type can not be found. Make sure it is loaded correctly and that the type still exists. FullTypeName = {assemblyQualifiedTypeName}. Full error message = {e}");
				}
			}
		}

		public HierarchyObject AttatchedTo
		{
			get
			{
				return HierarchyManager.FollowPath(attatchedToPath);
			}
		}
		#endregion

		#region Creators
		/// <summary>
		/// Deserializes the ScriptStorage from the provided binary file.
		/// </summary>
		/// <param name="path">The path to deserialize from.</param>
		/// <returns>The deserialized ScriptStorage.</returns>
		public static ScriptStorage CreateFromFile(string path)
		{
			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				return (ScriptStorage)binaryFormatter.Deserialize(stream);
			}
		}

		/// <summary>
		/// Creates a Script by deserializing the ScriptStorage at the provided path.
		/// </summary>
		/// <param name="path">The path to deserialize from.</param>
		/// <returns>The created Script.</returns>
		public static Script CreateScriptFromScriptStorageFile(string path)
		{
			return CreateFromFile(path).CreateScript();
		}

		/// <summary>
		/// Constructs the Script from the data in this ScriptStorage.
		/// </summary>
		/// <returns>The constructed Script.</returns>
		public Script CreateScript()
		{
			Script script = new Script(Type, constructorParameters, AttatchedTo); //TODO look up wether or not declaring a variable like this wastes any memory or if it is optimized. This does look cleaner than just returning I think, so for now it stays here and everywhere else!
			return script;
		}
		#endregion

		#region Storing
		/// <summary>
		/// Serializes and writes the ScriptStorage to a binary file using the BinaryFormatter.
		/// </summary>
		/// <param name="path">The path to store the ScriptStorage to.</param>
		/// <param name="toStore">The ScriptStorage to store.</param>
		public static void StoreToFile(string path, ScriptStorage toStore)
		{
			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				binaryFormatter.Serialize(stream, toStore);
			}
		}
		#endregion
	}
}