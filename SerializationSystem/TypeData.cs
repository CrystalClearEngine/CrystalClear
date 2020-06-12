using System;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	// TODO: turn into struct?
	[DataContract]
	public sealed class TypeData
	{
		public TypeData(Type type)
		{
			ConstructionTypeName = type.AssemblyQualifiedName;
		}

		public TypeData(string constructionTypeName)
		{
			ConstructionTypeName = constructionTypeName;
		}

		[DataMember]
		public string ConstructionTypeName { get; private set; }

		// TODO: determine if a cache for this is neccessary.
		public Type GetConstructionType() => Type.GetType(ConstructionTypeName, true);
	}
}