using System;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public class ImaginaryEnum
		: ImaginaryObject
	{
		public ImaginaryEnum(Type constructionType, string enumValue) : base(constructionType)
		{
			this.enumValue = enumValue;
		}

		private string enumValue;

		public override object CreateInstance()
		{
			return Enum.Parse(GetConstructionType(), enumValue);
		}
	}
}
