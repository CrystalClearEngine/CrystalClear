using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public sealed class ImaginaryEnum : ImaginaryObject, IGeneralImaginaryObject
	{
		public ImaginaryEnum(Type constructionType, string enumValue)
		{
			TypeData = new TypeData(constructionType);
			this.enumValue = enumValue;
		}

		internal ImaginaryEnum()
		{
		}

		[DataMember]
		public TypeData TypeData { get; set; }

		private string enumValue;

		public override object CreateInstance()
		{
			return Enum.Parse(TypeData.GetConstructionType(), enumValue);
		}

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			throw new NotImplementedException();
		}
	}
}
