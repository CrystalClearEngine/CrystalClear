using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public sealed class ImaginaryEnum : ImaginaryObject
	{
		public ImaginaryEnum(Type constructionType, string enumValue) : base(constructionType)
		{
			this.enumValue = enumValue;
		}

		internal ImaginaryEnum()
		{
		}

		private string enumValue;

		public override object CreateInstance()
		{
			return Enum.Parse(GetConstructionType(), enumValue);
		}

		internal override void WriteConstructionInfo(BinaryWriter writer, Encoding encoding)
		{
			throw new NotImplementedException();
		}

		internal override void ReadConstructionInfo(BinaryReader reader, Encoding encoding)
		{
			throw new NotImplementedException();
		}

		[DataMember]
		public string ConstructionTypeName { get; private set; }

		// TODO: determine if a cache for this is neccessary.
		public Type GetConstructionType()
		{
			return Type.GetType(ConstructionTypeName, true);
		}
	}
}
