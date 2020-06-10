using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using K4os.Compression.LZ4.Streams;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public static class ImaginaryObjectSerialization
	{
		public static TExpected LoadFromSaveFile<TExpected>(string path)
			where TExpected : ImaginaryObject
		{
			using (XmlReader readerStream = XmlReader.Create(path))
			{
				return (TExpected)new DataContractSerializer(typeof(TExpected)).ReadObject(readerStream);
			}
		}

		public static void SaveToFile(string path, ImaginaryObject toStore)
		{
			XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

			using (XmlWriter writerStream = XmlWriter.Create(path, settings))
			{
				new DataContractSerializer(toStore.GetType()).WriteObject(writerStream, toStore);
			}
		}

		public static ImaginaryObject UnpackImaginaryObject(string path)
		{
			Encoding encoding = Encoding.UTF8;

			using (FileStream fileStream = new FileStream(path, FileMode.Open))
#if DEBUG
			// BinaryReader without decompression if the program is being debugged.
			using (BinaryReader reader = new BinaryReader(fileStream, encoding))
#else
			using (LZ4DecoderStream decompressionStream = LZ4Stream.Decode(fileStream))
			using (BinaryReader reader = new BinaryReader(decompressionStream, encoding))
#endif
			{
				 // Read the version of CrystalClear that this pack file was created in.
				Version fileCreatedInVersion = new Version(reader.ReadString());

				// Is the pack file from an older version of CrystalClear?
				if (fileCreatedInVersion < CrystalClearInformation.CrystalClearVersion)
				{
					// The version that this file was created in is older than the current version.
					Console.WriteLine($"This file was created in an older version of the CrystalClear Engine. {fileCreatedInVersion} (file) < {CrystalClearInformation.CrystalClearVersion} (current)");
				}

				// Is the pack file from a newer version of CrystalClear?
				else if (fileCreatedInVersion > CrystalClearInformation.CrystalClearVersion)
				{
					// The version that this file was created in is newer than the current version.
					Console.WriteLine($"This file was created in a newer version of the CrystalClear Engine. {fileCreatedInVersion} (file) > {CrystalClearInformation.CrystalClearVersion} (current)");
				}

				ImaginaryObject unpacked;

				unpacked = ImaginaryObject.ReadImaginaryObject(reader, out _);

				return unpacked;
			}
		}

		public static void PackImaginaryObjectToFile(string path, ImaginaryObject toStore)
		{
			Encoding encoding = Encoding.UTF8;

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
#if DEBUG
			using (BinaryWriter writer = new BinaryWriter(fileStream, encoding))
#else
			using (LZ4EncoderStream compressionStream = LZ4Stream.Encode(fileStream))
			using (BinaryWriter writer = new BinaryWriter(compressionStream, encoding))
#endif
			{
				// Write the current CrystalClear version to the file.
				writer.Write(CrystalClearInformation.CrystalClearVersion.ToString());

				// Write the constructor data.
				toStore.WriteThis(writer);

				// Save the file.
				writer.Flush();
			}
		}
	}
}
