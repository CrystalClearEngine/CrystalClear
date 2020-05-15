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
			// Create an XmlReader stream to read the save file using the DataContractSerializer.
			using (XmlReader readerStream = XmlReader.Create(path))
			{
				// Deserialize the save file and then return the deserialized object.
				return (TExpected)new DataContractSerializer(typeof(TExpected)).ReadObject(readerStream);
			}
		}

		public static void SaveToFile(string path, ImaginaryObject toStore)
		{
			// Store the settings to be used for the serialization.
			XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

			// Create an XmlWriter stream to write to the save file using the DataContractSerializer.
			using (XmlWriter writerStream = XmlWriter.Create(path, settings))
			{
				// Create a new DataContractSerializer using the type from toStore.
				new DataContractSerializer(toStore.GetType()).WriteObject(writerStream, toStore);
			}
		}

		public static ImaginaryHierarchyObject UnpackHierarchyFromFile(string path)
		{
			// Store the encoding to use for serialization.
			Encoding encoding = Encoding.UTF8;

			// Create a fileStream to read from the pack file.
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
#if DEBUG
			// Create a BinaryReader that reads from read from fileStream without decompression if the program is being debugged.
			using (BinaryReader reader = new BinaryReader(fileStream))
#else
			// Create a LZ4 decompression stream to decompress the LZ4 compressed pack file.
			using (LZ4DecoderStream decompressionStream = LZ4Stream.Decode(fileStream))
			// Create a BinaryReader that reads from read from the compressionStream if the program is not being debugged.
			using (BinaryReader reader = new BinaryReader(decompressionStream))
#endif
			{ // TODO Create ReadImaginaryHierarchyObject and ReadImaginaryScript methods.
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

				ImaginaryHierarchyObject unpacked;

				string typeName = reader.ReadString();

				if (reader.ReadBoolean())
				{
					// Create a variable for storing the unpacked Hierarchy.
					 unpacked = new ImaginaryHierarchyObject(null,
													 Type.GetType(typeName, true),
													 ReadParameters());
				}
				else
				{
					unpacked = new ImaginaryHierarchyObject(null,
													Type.GetType(reader.ReadString(), true),
													ReadEditableData());
				}

				// Read and add the children to the unpacked Hierarchy.
				ReadAndAddChildren(unpacked);

				// Read and add the scripts to the unpacked Hierarchy.
				ReadAndAddScripts(unpacked);

				// Return the unpacked Hierarchy.
				return unpacked;

				// A utility method that reads children from a pack file and then adds them to the provided ImaginaryHierarchyObject.
				void ReadAndAddChildren(ImaginaryHierarchyObject parent)
				{
					// Get the number of children that this ImaginaryHierarchyObject has.
					int childCount = reader.ReadInt32();

					// Read and add as many times as there are children.
					for (int i = 0; i < childCount; i++)
					{
						// Read and store the child's name.
						string childName = reader.ReadString();

						ImaginaryHierarchyObject imaginaryHierarchyObject;

						string childTypeName = reader.ReadString();
						// Check if the HierarchyObject is using constructor parameters or is editable.
						if (reader.ReadBoolean())
						{ // This HierarchyObject is using constructor parameters.
							// Create, read and store a child.
							imaginaryHierarchyObject = new ImaginaryHierarchyObject(parent,
																						   Type.GetType(childTypeName, true),
																						   ReadParameters());
						}
						else
						{ // This HierarchyObject is editable.
							imaginaryHierarchyObject = new ImaginaryHierarchyObject(parent,
																						   Type.GetType(childTypeName, true),
																						   ReadEditableData());
						}

						// Add children to the ImahinaryHierarchyObject.
						ReadAndAddChildren(imaginaryHierarchyObject);

						// Add the child to the parent.
						parent.LocalHierarchy.Add(childName, imaginaryHierarchyObject);
					}
				}

				// A utility method that recursively reads the attatchedScripts from a pack file and then adds them to the provided ImaginaryHierarchyObject.
				void ReadAndAddScripts(ImaginaryHierarchyObject toAddTo)
				{
					// Get the number of attatched scripts that this ImaginaryHierarchyObject has.
					int scriptCount = reader.ReadInt32();

					// Read and add as many times as there are scripts.
					for (int i = 0; i < scriptCount; i++)
					{
						// Read and store the script's name.
						string scriptName = reader.ReadString();

						// Create, read and store an ImaginaryScript.
						ImaginaryScript imaginaryScript = new ImaginaryScript(
							Type.GetType(reader.ReadString(), true),
							ReadParameters());

						toAddTo.AttatchedScripts.Add(scriptName, imaginaryScript);
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

						if (ImaginaryPrimitive.QualifiesAsImaginaryPrimitive(type))
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

				EditorData ReadEditableData()
				{
					EditorData editorData = EditorData.GetEmpty();

					int editableDataLength = reader.ReadInt32();

					if (editableDataLength == 0)
					{
						return editorData;
					}

					for (int i = 0; i < editableDataLength; i++)
					{
						editorData.Add(reader.ReadString(), reader.ReadString());
					}

					return editorData;
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
				// Write the current CrystalClear version to the file.
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
					// Write the number of scripts so the reader doesn't read too much.
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
