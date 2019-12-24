using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// HierarchyStorage is a type specifically for allowing serialization and deserialization of Hierarchies easily.
	/// </summary>
	[DataContract(Name = "Hierarchy")] // For the DataContractSerializer...
	[Serializable] // For the BinaryFormatter...
	public class HierarchyStorage
	{
		#region Data
		[DataMember(Name = "Hierarchy")]
		public Hierarchy hierarchy;
		#endregion

		#region Creators

		/// <summary>
		/// Deserializes the HierarchyStorage from the provided binary file.
		/// </summary>
		/// <param name="path">The path to deserialize from.</param>
		/// <returns>The deserialized HierarchyStorage.</returns>
		public static HierarchyStorage CreateFromFile(string path)
		{
			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				return (HierarchyStorage)binaryFormatter.Deserialize(stream);
			}
		}

		/// <summary>
		/// Creates a Hierarchy by deserializing the HierarchyStorage at the provided path.
		/// </summary>
		/// <param name="path">The path to deserialize from.</param>
		/// <returns>The created Hierarchy.</returns>
		public static Hierarchy CreateHierarchyFromHierarchyStorageFile(string path)
		{
			return CreateFromFile(path).CreateHierarchy();
		}

		/// <summary>
		/// Constructs the Hierarchy from the data in this HierarchyStorage.
		/// </summary>
		/// <returns>The constructed Hierarchy.</returns>
		public Hierarchy CreateHierarchy()
		{
			return hierarchy;
		}
		#endregion

		#region Storing
		/// <summary>
		/// Serializes and writes the HierarchyStorage to a binary file using the BinaryFormatter.
		/// </summary>
		/// <param name="path">The path to store the HierarchyStorage to.</param>
		/// <param name="toStore">The HierarchyStorage to store.</param>
		public static void StoreToFile(string path, HierarchyStorage toStore)
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
