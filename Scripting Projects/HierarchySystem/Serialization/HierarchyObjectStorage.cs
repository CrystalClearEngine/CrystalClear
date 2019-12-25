using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// HierarchyObjectStorage is a type specifically for allowing serialization and deserialization of HierarchyObjects.
	/// </summary>
	[DataContract(Name = "HierarchyObject")] // For the DataContractSerializer...
	[Serializable] // For the BinaryFormatter...
	public class HierarchyObjectStorage
	{
		public HierarchyObjectStorage()
		{

		}

		public HierarchyObjectStorage(HierarchyObjectStorage parent)
		{

		}

		#region Data
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
				return HierarchyManager.FollowPath(path);
			}
		}
		#endregion

		#region Creators
		public Script[] CreateScripts()
		{
			List<Script> scripts = new List<Script>();
			foreach (ScriptStorage scriptStorage in attatchedScripts)
			{
				scripts.Add(scriptStorage.CreateScript());
			}
			return scripts.ToArray();
		}

		/// <summary>
		/// Deserializes the HierarchyObjectStorage from the provided binary file.
		/// </summary>
		/// <param name="path">The path to deserialize from.</param>
		/// <returns>The deserialized HierarchyObjectStorage.</returns>
		public static HierarchyObjectStorage CreateFromFile(string path)
		{
			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				return (HierarchyObjectStorage)binaryFormatter.Deserialize(stream);
			}
		}

		/// <summary>
		/// Creates a HierarchyObject by deserializing the HierarchyObjectStorage at the provided path.
		/// </summary>
		/// <param name="path">The path to deserialize from.</param>
		/// <returns>The created HierarchyObject.</returns>
		public static HierarchyObject CreateHierarchyObjectFromHierarchyObjectStorageFile(string path)
		{
			return CreateFromFile(path).CreateHierarchyObject();
		}

		/// <summary>
		/// Constructs the HierarchyObject from the data in this HierarchyObjectStorage.
		/// </summary>
		/// <returns>The constructed HierarchyObject.</returns>
		public HierarchyObject CreateHierarchyObject()
		{
			HierarchyObject hierarchyObject;
			if (constructorParameters != null)
			{
				hierarchyObject = (HierarchyObject)Activator.CreateInstance(Type, constructorParameters);
			}
			else
			{
				hierarchyObject = (HierarchyObject)Activator.CreateInstance(Type);
			}
			hierarchyObject.AttatchedScripts.AddRange(CreateScripts());
			return hierarchyObject;
		}
		#endregion

		#region Storing
		/// <summary>
		/// Serializes and writes the HierarchyObjectStorage to a binary file using the BinaryFormatter.
		/// </summary>
		/// <param name="path">The path to store the HierarchyObjectStorage to.</param>
		/// <param name="toStore">The HierarchyObjectStorage to store.</param>
		public static void StoreToFile(string path, HierarchyObjectStorage toStore)
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
