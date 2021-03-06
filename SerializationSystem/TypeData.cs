﻿using System;
using System.IO;
using System.Reflection;
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

		[DataMember] public string ConstructionTypeName { get; private set; }

		public void WriteConstructionInfo(BinaryWriter writer)
		{
			writer.Write(ConstructionTypeName);
		}

		public void ReadConstructionInfo(BinaryReader reader)
		{
			ConstructionTypeName = reader.ReadString();
		}

		// TODO: determine if a cache for this is neccessary.
		public Type GetConstructionType() => Type.GetType(ConstructionTypeName,
		assemblyResolver: delegate (AssemblyName assemblyName) // A custom assemblyResolver is needed because the assembly may be in another AssemblyLoadContext.
		{ // TODO: keep a list of all AssemblyLoadContexts and look through them instead, so all types can be detected?
		  // TODO: turn this whole call into an extension for Type?
			foreach (var assembly in RuntimeInformation.UserAssemblies)
			{
				if (assembly.GetName().FullName == assemblyName.FullName)
				{
					// TODO: keep a list of all AssemblyLoadContexts and look through them instead, so all types can be detected?
					// TODO: turn this whole call into an extension for Type?
					return assembly;
				}
			}

			return null;
		},
		null,
		true);
	}
}