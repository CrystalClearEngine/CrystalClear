using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// HierarchyObjectStorage is a type specifically for allowing serialization and deserialization of HierarchyObjects.
	/// </summary>
	[Serializable] // For the binary formatter...
	public class HierarchyObjectStorage
	{
		public HierarchyObjectStorage(Type HierarchyObjectType, HierarchyObject parent, object[] constructorParameters = null)
		{
			this.assemblyQualifiedTypeName = HierarchyObjectType.AssemblyQualifiedName;
			this.constructorParameters = constructorParameters;
			this.path = parent.Path;
			List<ScriptStorage> scriptStorages = new List<ScriptStorage>();
			foreach (Script script in parent.AttatchedScripts)
			{
				scriptStorages.Add(
					new ScriptStorage(
						script.GetType()));
			}
			this.attatchedScripts = scriptStorages.ToArray();
		}

		private readonly ScriptStorage[] attatchedScripts;
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
		/// The assemblyQualifiedName of the type of the HierarchyObject.
		/// </summary>
		private readonly string assemblyQualifiedTypeName;
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

		/// <summary>
		/// The parameters to use when constructing this HierarchyObject.
		/// </summary>
		private readonly object[] constructorParameters;

		/// <summary>
		/// The path to follow from HierarchyManager to find the position for this HierarchyObject to be added to.
		/// </summary>
		private readonly string path;
		public HierarchyObject Parent
		{
			get
			{
				return HierarchyManager.FollowPath(path);
			}
		}

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
	}
}