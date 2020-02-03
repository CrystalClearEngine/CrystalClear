using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem
{
	[Serializable]
	[DataContract]
	public class ImaginaryObject
	{
		public ImaginaryObject(Type constructionType, ImaginaryObject[] constructorParams)
		{
			ConstructionTypeName = constructionType.AssemblyQualifiedName;
			ConstructionParameters = constructorParams ?? Array.Empty<ImaginaryObject>();
		}

		protected ImaginaryObject()
		{

		}

		[DataMember]
		public string ConstructionTypeName { get; set; }

		[DataMember]
		public ImaginaryObject[] ConstructionParameters;

		public Type GetConstructionType()
		{
			return Type.GetType(ConstructionTypeName, true);
		}

		internal void WriteConstructionInfo(BinaryWriter binaryWriter, Encoding encoding)
		{
			binaryWriter.Write(ConstructionTypeName);
			binaryWriter.Write(ConstructionParameters.Length);
			foreach (ImaginaryObject imaginary in ConstructionParameters)
			{
				binaryWriter.Write(imaginary.ConstructionTypeName);

				if (imaginary.GetConstructionType().IsPrimitive)
					binaryWriter.Write((imaginary as ImaginaryPrimitive).StringValue);
				else
					imaginary.WriteConstructionInfo(binaryWriter, encoding);
			}
		}
	}
}
