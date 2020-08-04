using System;
using System.IO;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public sealed class ImaginaryEnum : ImaginaryObject, IGeneralImaginaryObject
	{
		private string enumValue;

		public ImaginaryEnum(Type constructionType, string enumValue)
		{
			TypeData = new TypeData(constructionType);
			this.enumValue = enumValue;
		}

		public ImaginaryEnum()
		{
		}

		[DataMember] public TypeData TypeData { get; set; }

		public override object CreateInstance() => Enum.Parse(TypeData.GetConstructionType(), enumValue);

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			TypeData.WriteConstructionInfo(writer);

			writer.Write(enumValue);
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			TypeData = new TypeData(reader);

			enumValue = reader.ReadString();
		}
	}
}