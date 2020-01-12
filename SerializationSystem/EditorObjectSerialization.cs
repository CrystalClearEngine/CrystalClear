using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using K4os.Compression.LZ4.Streams;

namespace CrystalClear.SerializationSystem
{
	public static class EditorObjectSerialization
	{
		public static EditorObject CreateFromSaveFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(EditorObject));

				EditorObject deserializedEditorObject = (EditorObject)dataContractSerializer.ReadObject(fileStream);

				return deserializedEditorObject;
			}
		}

		public static void SaveToFile(string path, EditorObject toStore)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(EditorObject));

				dataContractSerializer.WriteObject(fileStream, toStore);
			}
		}

		public static EditorObject CreateFromStoreFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			using (LZ4DecoderStream decompressionStream = LZ4Stream.Decode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				EditorObject deserializedEditorObject = (EditorObject)binaryFormatter.Deserialize(decompressionStream);

				return deserializedEditorObject;
			}
		}

		public static void StoreToFile(string path, EditorObject toStore)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			using (LZ4EncoderStream compressionStream = LZ4Stream.Encode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				binaryFormatter.Serialize(compressionStream, toStore);
			}
		}
	}
}
