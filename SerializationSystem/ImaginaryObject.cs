using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	[Serializable]
	[DataContract]
	public class ImaginaryObject
	{
		public ImaginaryObject(Type constructionType, ImaginaryObject[] constructorParams)
		{
			ConstructionTypeName = constructionType.AssemblyQualifiedName;
			ConstructorParams = constructorParams ?? Array.Empty<ImaginaryObject>();
		}

		protected ImaginaryObject()
		{

		}

		[DataMember]
		public string ConstructionTypeName { get; set; }
		[DataMember]
		public ImaginaryObject[] ConstructorParams;

		public Type GetConstructionType()
		{
			return Type.GetType(ConstructionTypeName, true);
		}
	}
}
