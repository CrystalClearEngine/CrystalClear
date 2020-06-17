﻿using System;
using System.IO;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	// TODO: turn into struct?
	[DataContract]
	public sealed class TypeData : IBinarySerializable
	{
		public TypeData(Type type)
		{
			ConstructionTypeName = type.AssemblyQualifiedName;
		}

		public TypeData(string constructionTypeName)
		{
			ConstructionTypeName = constructionTypeName;
		}

		public TypeData(BinaryReader reader)
		{
			ReadConstructionInfo(reader);
		}

		public void WriteConstructionInfo(BinaryWriter writer)
		{
			writer.Write(ConstructionTypeName);
		}

		public void ReadConstructionInfo(BinaryReader reader)
		{
			ConstructionTypeName = reader.ReadString();
		}

		[DataMember]
		public string ConstructionTypeName { get; private set; }

		// TODO: determine if a cache for this is neccessary.
		public Type GetConstructionType() => Type.GetType(ConstructionTypeName, true);
	}
}