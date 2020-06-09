using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public static class ImaginaryPrimitiveExtensions
	{
		public static bool QualifiesAsPrimitive(this object valueToCheck)
			=> QualifiesAsImaginaryPrimitive(valueToCheck.GetType());

		public static bool QualifiesAsImaginaryPrimitive(this Type type)
			=> type.IsPrimitive
				|| type == typeof(string)
				|| !type.IsEnum
				|| type.IsAssignableFrom(typeof(string))
				|| type.IsAssignableFrom(typeof(IFormattable))
				|| type.IsAssignableFrom(typeof(IConvertible));
	}

	[DataContract]
	public sealed class ImaginaryPrimitive : ImaginaryObject
	{
		public ImaginaryPrimitive(object value)
		{
			if (!value.QualifiesAsPrimitive())
			{
				throw new ArgumentException($"{value.GetType().FullName} does not qualify as an ImaginaryPrimitive."); // TODO: use custom exception.
			}

			TypeData = new TypeData(value.GetType());

			PrimitiveObjectValue = value;
		}

		internal ImaginaryPrimitive()
		{
		}

		[DataMember]
		public TypeData TypeData;

		[DataMember]
		public object PrimitiveObjectValue;

		public string StringValue => Convert.ToString(PrimitiveObjectValue);

		public override string ToString()
		{
			return StringValue;
		}

		public override object CreateInstance()
		{
			return PrimitiveObjectValue;
		}

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			writer.Write(StringValue);
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			PrimitiveObjectValue = reader.ReadString();
		}
	}
}
