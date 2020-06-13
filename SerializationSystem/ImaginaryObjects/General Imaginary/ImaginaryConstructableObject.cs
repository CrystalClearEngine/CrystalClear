﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	// TODO: Constructable or Constructible?

	/// <summary>
	/// An ImaginaryObject is an object that stores the construction or editor data for the object so that it can be created in the editor, serialized and finally deserialized and an instance can be created.
	/// </summary>
	[DataContract]
	public sealed class ImaginaryConstructableObject : ImaginaryObject, IGeneralImaginaryObject
	{
		/// <summary>
		/// Creates an ImaginaryObject with the specified type and constructor parameters.
		/// </summary>
		/// <param name="constructionType">The type of object to represent.</param>
		/// <param name="constructorParameters">The constructor parameters to use initially.</param>
		public ImaginaryConstructableObject(Type constructionType, ImaginaryObject[] constructorParameters = null)
		{
			TypeData = new TypeData(constructionType.AssemblyQualifiedName);
			ImaginaryConstructionParameters = constructorParameters ?? Array.Empty<ImaginaryObject>();
		}

		private ImaginaryConstructableObject()
		{ }

		[DataMember]
		public TypeData TypeData { get; set; }

		/// <summary>
		/// The parameters to be used when constructing the object.
		/// </summary>
		[DataMember]
		public ImaginaryObject[] ImaginaryConstructionParameters;

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

		public override object CreateInstance()
		{
			return Activator.CreateInstance(TypeData.GetConstructionType(), GetImaginaryConstructionParameters());
		}

		/// <summary>
		/// Writes the construction info of the object using the provided BinaryWriter and encoding.
		/// </summary>
		/// <param name="writer">The BinaryWriter to use to write the data to.</param>
		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			writer.Write(TypeData.ConstructionTypeName);
			
			// TODO: use Write7BitEncodedInt?
			writer.Write(ImaginaryConstructionParameters.Length);

			foreach (ImaginaryObject parameter in ImaginaryConstructionParameters)
			{
				WriteImaginaryObject(parameter, writer);
			}
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			TypeData = new TypeData(reader.ReadString());

			ImaginaryConstructionParameters = new ImaginaryObject[reader.ReadInt32()];

			for (int i = 0; i < ImaginaryConstructionParameters.Length; i++)
			{
				ImaginaryConstructionParameters[i] = ReadImaginaryObject(reader, out _);
			}
		}
	}
}
