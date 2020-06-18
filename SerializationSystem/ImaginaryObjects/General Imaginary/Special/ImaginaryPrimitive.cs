using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[EditorBrowsable(EditorBrowsableState.Never)] // TODO: determine whether or not this will also hide the contained extension methods, in which case it should be removed.
	public static class ImaginaryPrimitiveExtensions // Extensions are kept here because they cannot be placed in the generic ImaginaryPrimitive class.
	{
		public static bool QualifiesAsPrimitive(this object valueToCheck)
			=> QualifiesAsPrimitive(valueToCheck.GetType());

		public static bool QualifiesAsPrimitive(this Type type)
			=> type.IsPrimitive
				|| type == typeof(string)
				|| !type.IsEnum
				|| type.IsAssignableFrom(typeof(string))
				|| type.IsAssignableFrom(typeof(IFormattable))
				|| type.IsAssignableFrom(typeof(IConvertible));
	}

	[DataContract]
	public sealed class ImaginaryPrimitive : ImaginaryObject, IGeneralImaginaryObject
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

		public ImaginaryPrimitive()
		{
		}

		[DataMember]
		public TypeData TypeData { get; set; }

		[DataMember]
		public object PrimitiveObjectValue;

		public string StringValue => Convert.ToString(PrimitiveObjectValue);

		public override string ToString() => StringValue;

		public override object CreateInstance() => PrimitiveObjectValue;

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			TypeData.WriteConstructionInfo(writer);

			writer.Write(StringValue);
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			TypeData = new TypeData(reader);

			PrimitiveObjectValue = reader.ReadString();
		}
	}
}
