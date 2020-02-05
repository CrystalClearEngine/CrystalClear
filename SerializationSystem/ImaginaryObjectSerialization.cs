using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace CrystalClear.SerializationSystem
{
	public static class ImaginaryObjectSerialization
	{
		public static TExpected LoadFromSaveFile<TExpected>(string path)
			where TExpected : ImaginaryObject
		{
			using (XmlReader readerStream = XmlReader.Create(path))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(TExpected));

				TExpected deserializedEditorObject = (TExpected)dataContractSerializer.ReadObject(readerStream);

				return deserializedEditorObject;
			}
		}

		public static void SaveToFile(string path, ImaginaryObject toStore)
		{
			XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

			using (XmlWriter writerStream = XmlWriter.Create(path, settings))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(toStore.GetType());

				dataContractSerializer.WriteObject(writerStream, toStore);
			}
		}

		public static ImaginaryHierarchyObject UnpackHierarchyFromFile(string path)
		{
			Encoding encoding = Encoding.UTF8;

			using (FileStream fileStream = new FileStream(path, FileMode.Open))
#if DEBUG
			using (BinaryReader reader = new BinaryReader(fileStream))
#else
			using (LZ4DecoderStream decompressionStream = LZ4Stream.Decode(fileStream))
			using (BinaryReader reader = new BinaryReader(decompressionStream))
#endif
			{
				Version fileCreatedInVersion = new Version(reader.ReadString());

				if (fileCreatedInVersion < CrystalClearInformation.CrystalClearVersion)
				{
					// The version that this file was created in is older than the current version.
					Console.WriteLine($"This file was created in an older version of the Crystal Clear Engine. {fileCreatedInVersion} (file) > {CrystalClearInformation.CrystalClearVersion} (current)");
				}

				else if (fileCreatedInVersion > CrystalClearInformation.CrystalClearVersion)
				{
					// The version that this file was created in is newer than the current version.
					Console.WriteLine($"This file was created in a newer version of the Crystal Clear Engine. {fileCreatedInVersion} (file) < {CrystalClearInformation.CrystalClearVersion} (current)");
				}

				ImaginaryHierarchyObject unpacked = new ImaginaryHierarchyObject(null, Type.GetType(reader.ReadString(), true), ReadParameters());

				ReadAndAddChildren(unpacked);

				ReadAndAddScripts(unpacked);

				return unpacked;

				void ReadAndAddChildren(ImaginaryHierarchyObject parent)
				{
					int childCount = reader.ReadInt32();

					for (int i = 0; i < childCount; i++)
					{
						string childName = reader.ReadString();
						ImaginaryHierarchyObject imaginaryHierarchyObject = new ImaginaryHierarchyObject(parent, Type.GetType(reader.ReadString(), true), ReadParameters());
						parent.LocalHierarchy.Add(childName, imaginaryHierarchyObject);
						ReadAndAddChildren(imaginaryHierarchyObject);
					}
				}

				void ReadAndAddScripts(ImaginaryHierarchyObject toAddTo)
				{
					int scriptCount = reader.ReadInt32();

					for (int i = 0; i < scriptCount; i++)
					{
						string childName = reader.ReadString();
						ImaginaryScript imaginaryScript = new ImaginaryScript(Type.GetType(reader.ReadString(), true), ReadParameters());
						toAddTo.AttatchedScripts.Add(childName, imaginaryScript);
					}
				}

				ImaginaryObject[] ReadParameters()
				{
					int parametersLength = reader.ReadInt32();

					if (parametersLength == 0)
					{
						return Array.Empty<ImaginaryObject>();
					}

					List<ImaginaryObject> parameters = new List<ImaginaryObject>();

					for (int i = 0; i < parametersLength; i++)
					{
						Type type = Type.GetType(reader.ReadString(), true);

						if (ImaginaryPrimitive.QualifiesAsPrimitive(type))
						{
							parameters.Add(new ImaginaryPrimitive(Convert.ChangeType(reader.ReadString(), type)));
						}
						else
						{
							parameters.Add(new ImaginaryObject(type, ReadParameters()));
						}
					}

					return parameters.ToArray();
				}
			}
		}

		public static void PackHierarchyToFile(string path, ImaginaryHierarchyObject toStore)
		{
			Encoding encoding = Encoding.UTF8;

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
#if DEBUG
			using (BinaryWriter writer = new BinaryWriter(fileStream))
#else
			using (LZ4EncoderStream compressionStream = LZ4Stream.Encode(fileStream))
			using (BinaryWriter writer = new BinaryWriter(compressionStream))
#endif
			{
				// Write the current Crystal Clear version to the file.
				writer.Write(CrystalClearInformation.CrystalClearVersion.ToString());

				// Write the constructor data.
				toStore.WriteConstructionInfo(writer, encoding);

				// Write the LocalHierarchy.
				WriteLocalHierarchy(toStore);
				WriteAttatchedScripts(toStore);

				// Save the file.
				writer.Flush();

				void WriteLocalHierarchy(ImaginaryHierarchyObject toWrite)
				{
					// Write the number of children.
					writer.Write(toWrite.LocalHierarchy.Count);
					// Iterate all children.
					foreach (KeyValuePair<string, ImaginaryHierarchyObject> child in toWrite.LocalHierarchy)
					{
						// Write the child's name.
						writer.Write(child.Key);
						// Write the child's constructor info.
						child.Value.WriteConstructionInfo(writer, encoding);
						// Write the child's LocalHierarchy.
						WriteLocalHierarchy(child.Value);
					}
				}

				void WriteAttatchedScripts(ImaginaryHierarchyObject toWrite)
				{
					// Write the number of scripts.
					writer.Write(toWrite.AttatchedScripts.Count);
					// Iterate all scripts.
					foreach (KeyValuePair<string, ImaginaryScript> script in toWrite.AttatchedScripts)
					{
						// Write the script's name.
						writer.Write(script.Key);
						// Write the script's constructor info.
						script.Value.WriteConstructionInfo(writer, encoding);
					}
				}
			}
		}
	}
}
