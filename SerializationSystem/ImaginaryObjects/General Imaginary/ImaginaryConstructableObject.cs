using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	// TODO: Constructable or Constructible?

	/// <summary>
	///     An ImaginaryObject is an object that stores the construction or editor data for the object so that it can be
	///     created in the editor, serialized and finally deserialized and an instance can be created.
	/// </summary>
	[DataContract] // TODO: wait does this support generic types? Don't think so, that should be resolved.
	public sealed class ImaginaryConstructableObject : ImaginaryObject, IGeneralImaginaryObject
	{
		/// <summary>
		///     The parameters to be used when constructing the object.
		/// </summary>
		[DataMember] public ImaginaryObject[] ImaginaryConstructionParameters;

		/// <summary>
		///     Creates an ImaginaryObject with the specified type and constructor parameters.
		/// </summary>
		/// <param name="constructionType">The type of object to represent.</param>
		/// <param name="constructorParameters">The constructor parameters to use initially.</param>
		public ImaginaryConstructableObject(Type constructionType, ImaginaryObject[] constructorParameters = null)
		{
			TypeData = new TypeData(constructionType.AssemblyQualifiedName);
			ImaginaryConstructionParameters = constructorParameters ?? Array.Empty<ImaginaryObject>();
		}

		public ImaginaryConstructableObject()
		{
		}

		[DataMember] public TypeData TypeData { get; set; }

		public override object CreateInstance() => Activator.CreateInstance(TypeData.GetConstructionType(),
			GetImaginaryConstructionParameters());

		public object[] GetImaginaryConstructionParameters()
		{
			object[] imaginaryConstructionParameters = new object[ImaginaryConstructionParameters.Length];

			for (int i = 0; i < ImaginaryConstructionParameters.Length; i++)
			{
				ImaginaryObject imaginaryObject = ImaginaryConstructionParameters[i];
				imaginaryConstructionParameters[i] = imaginaryObject.CreateInstance();
			}

			return imaginaryConstructionParameters;
		}

		/// <summary>
		///     Writes the construction info of the object using the provided BinaryWriter and encoding.
		/// </summary>
		/// <param name="writer">The BinaryWriter to use to write the data to.</param>
		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			TypeData.WriteConstructionInfo(writer);

			// TODO: use Write7BitEncodedInt?
			writer.Write(ImaginaryConstructionParameters.Length);

			foreach (ImaginaryObject parameter in ImaginaryConstructionParameters)
			{
				WriteImaginaryObject(parameter, writer);
			}
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			TypeData = new TypeData(reader);

			ImaginaryConstructionParameters = new ImaginaryObject[reader.ReadInt32()];

			for (var i = 0; i < ImaginaryConstructionParameters.Length; i++)
			{
				ImaginaryConstructionParameters[i] = ReadImaginaryObject(reader, out _);
			}
		}
	}
}