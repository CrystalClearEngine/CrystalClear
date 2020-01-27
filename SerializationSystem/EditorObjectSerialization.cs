﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using K4os.Compression.LZ4.Streams;

namespace CrystalClear.SerializationSystem
{
	public static class EditorObjectSerialization
	// TODO maybe make multi purpose? reasoning being that a load method already has an expectedType parameter.
	{
		// TODO maybe add generic version, where return type is expectedType.
		public static EditorObject LoadFromSaveFile(string path, Type expectedType)
		{
			using (XmlReader readerStream = XmlReader.Create(path))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(expectedType);

				EditorObject deserializedEditorObject = (EditorObject)dataContractSerializer.ReadObject(readerStream);

				return deserializedEditorObject;
			}
		}

		public static void SaveToFile(string path, EditorObject toStore)
		{
			var settings = new XmlWriterSettings { Indent = true };

			using (XmlWriter writerStream = XmlWriter.Create(path, settings))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(toStore.GetType());

				dataContractSerializer.WriteObject(writerStream, toStore);
			}
		}

		public static EditorObject UnpackFromFile(string path) // Use custom binaryWriter powered serializer.
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			using (LZ4DecoderStream decompressionStream = LZ4Stream.Decode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				EditorObject deserializedEditorObject = (EditorObject)binaryFormatter.Deserialize(decompressionStream);

				return deserializedEditorObject;
			}
		}

		public static void PackToFile(string path, EditorObject toStore)
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
