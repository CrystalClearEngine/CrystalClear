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

			PrimitiveObjectValue = value;
		}

		internal ImaginaryPrimitive()
		{
		}

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

		internal override void WriteConstructionInfo(BinaryWriter writer, Encoding encoding)
		{
			writer.Write(StringValue);
		}

		internal override void ReadConstructionInfo(BinaryReader reader, Encoding encoding)
		{
			PrimitiveObjectValue = reader.ReadString();
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
