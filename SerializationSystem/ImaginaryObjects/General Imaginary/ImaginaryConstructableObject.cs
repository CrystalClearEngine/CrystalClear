using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	/// <summary>
	/// An ImaginaryObject is an object that stores the construction or editor data for the object so that it can be created in the editor, serialized and finally deserialized and an instance can be created.
	/// </summary>
	[DataContract]
	public sealed class ImaginaryConstructableObject : ImaginaryGeneralObject
	{
		/// <summary>
		/// Creates an ImaginaryObject with the specified type and constructor parameters.
		/// </summary>
		/// <param name="constructionType">The type of object to represent.</param>
		/// <param name="constructorParameters">The constructor parameters to use initially.</param>
		public ImaginaryConstructableObject(Type constructionType, ImaginaryObject[] constructorParameters = null)
		{
			ConstructionTypeName = constructionType.AssemblyQualifiedName;
			ImaginaryConstructionParameters = constructorParameters ?? Array.Empty<ImaginaryObject>();
		}

		internal ImaginaryConstructableObject()
		{
		}

		/// <summary>
		/// The parameters to be used when constructing the object.
		/// </summary>
		[DataMember]
		public ImaginaryObject[] ImaginaryConstructionParameters;

		public override object CreateInstance()
		{
			List<object> constructorObjects = new List<object>();

			foreach (ImaginaryObject imaginaryObject in ImaginaryConstructionParameters)
			{
				constructorObjects.Add(imaginaryObject.CreateInstance());
			}

			return Activator.CreateInstance(GetConstructionType(), constructorObjects.ToArray());
		}

		/// <summary>
		/// Writes the construction info of the object using the provided BinaryWriter and encoding.
		/// </summary>
		/// <param name="writer">The BinaryWriter to use to write the data to.</param>
		/// <param name="encoding">The encoding to use for writing data.</param>
		internal override void WriteConstructionInfo(BinaryWriter writer, Encoding encoding)
		{
			writer.Write(ConstructionTypeName);
			
			// TODO: use Write7BitEncodedInt?
			writer.Write(ImaginaryConstructionParameters.Length);

			foreach (ImaginaryObject parameter in ImaginaryConstructionParameters)
			{
				parameter.WriteConstructionInfo(writer, encoding);
			}
		}

		internal override void ReadConstructionInfo(BinaryReader reader, Encoding encoding)
		{
			ConstructionTypeName = reader.ReadString();

			ImaginaryConstructionParameters = new ImaginaryObject[reader.ReadInt32()];

			for (int i = 0; i < ImaginaryConstructionParameters.Length; i++)
			{
				ImaginaryConstructionParameters[i] = ImaginaryGeneralObject.ReadConstructionInfo(reader, encoding);
			}
		}
	}
}
