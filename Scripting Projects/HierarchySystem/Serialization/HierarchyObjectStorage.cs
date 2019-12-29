using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// HierarchyObjectStorage is a type specifically for allowing serialization and deserialization of HierarchyObjects.
	/// </summary>
	[DataContract(Name = "HierarchyObject")] // For the DataContractSerializer...
	[Serializable] // For the BinaryFormatter...
	public class HierarchyObjectStorage
	{	
		/// <summary>
		/// Used for creating an HierarchyObject when later deserialized.
		/// </summary>
		public HierarchyObjectStorage(HierarchyObjectStorage parent = null)
		{
		}

		public HierarchyObjectStorage(HierarchyStorage hierarchy, ScriptStorage[] scripts, string assemblyQualifiedTypeName, object[] constructorParameters, HierarchyObjectStorage parent = null)
		{

		}

		#region Data
		[DataMember(Name = "Hierarchy")]
		private readonly Hierarchy hierarchy;

		[DataMember(Name = "Scripts")]
		private readonly ScriptStorage[] attatchedScripts;

		/// <summary>
		/// The assemblyQualifiedName of the type of the HierarchyObject.
		/// </summary>
		[DataMember(Name = "HierarchyObjectTypeName")]
		private readonly string assemblyQualifiedTypeName;

		/// <summary>
		/// The parameters to use when constructing this HierarchyObject.
		/// </summary>
		[DataMember(Name = "ConstructorParameters")]
		private readonly object[] constructorParameters;

		/// <summary>
		/// The path to follow from HierarchyManager to find the position for this HierarchyObject to be added to.
		/// </summary>
		[DataMember(Name = "Path")]
		private readonly string path;
		#endregion

		#region Properties
		public Type Type
		{
			get
			{
				try
				{
					return Type.GetType(assemblyQualifiedTypeName); // TODO store this in a cache
				}
				catch (TypeLoadException e)
				{
					throw new Exception($"The specified HierarchyObject type can not be found. Make sure it is loaded correctly and that the type still exists. FullTypeName = {assemblyQualifiedTypeName}. Full error message = {e}");
				}
			}
		}

		public HierarchyObject Parent
		{
			get
			{
				return HierarchyManager.FollowPath(path); // TODO also store this in a cache
			}
		}
		#endregion

		#region Creators
		//public Script[] CreateScripts()
		//{
		//	List<Script> scripts = new List<Script>();
		//	foreach (ScriptStorage scriptStorage in attatchedScripts)
		//	{
		//		scripts.Add(scriptStorage.CreateScript());
		//	}
		//	return scripts.ToArray();
		//}

		/// <summary>
		/// Deserializes the HierarchyObjectStorage from the provided binary file using the BinaryFormatter.
		/// </summary>
		/// <param name="path">The path to deserialize from.</param>
		/// <returns>The deserialized HierarchyObject.</returns>
		public static HierarchyObject CreateFromFile(string path)
		{
			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				return ((HierarchyObjectStorage)
					binaryFormatter
					.Deserialize(stream))
					.CreateHierarchyObject();
			}
		}

		/// <summary>
		/// Constructs the HierarchyObject from the data in this HierarchyObjectStorage.
		/// </summary>
		/// <returns>The constructed HierarchyObject.</returns>
		public HierarchyObject CreateHierarchyObject()
		{
			// Declare instance.
			HierarchyObject hierarchyObject;

			// There is a performance penalty to creating an instance with parameters using Activator, so this avoids that if there are no parameters to use!
			if (constructorParameters != null)
			{
				hierarchyObject = (HierarchyObject)Activator.CreateInstance(Type, constructorParameters);
			}
			else
			{
				hierarchyObject = (HierarchyObject)Activator.CreateInstance(Type);
			}

			foreach (ScriptStorage script in attatchedScripts)
			{
				//hierarchyObject.AddScript(script.Type, script.constructorParameters);
			}

			// Return the fully constructed HierarchyObject.
			return hierarchyObject;
		}
		#endregion

		#region Storing
		/// <summary>
		/// Exports the HierarchyObjectStorage to a binary file using the BinaryFormatter.
		/// </summary>
		/// <param name="path">The path to export the HierarchyObjectStorage to.</param>
		/// <param name="toExport">The HierarchyObjectStorage to export.</param>
		public static void ExportToFile(string path, HierarchyObjectStorage toExport)
		{
			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				binaryFormatter.Serialize(stream, toExport);
			}
		}

		/// <summary>
		/// Serializes and stores the HierarchyObjectStorage to an XML file using the DataContractSerializer.
		/// </summary>
		/// <param name="path">Where the HierarchyObjectStorage should be stored to.</param>
		/// <param name="toStore">The HierarchyObjectStorage to store.</param>
		public static void StoreToFile(string path, HierarchyObjectStorage toStore)
		{
			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				DataContractSerializer serializer = new DataContractSerializer(typeof(HierarchyObjectStorage));

				serializer.WriteObject(stream, toStore);
			}
		}
		#endregion
	}
}
