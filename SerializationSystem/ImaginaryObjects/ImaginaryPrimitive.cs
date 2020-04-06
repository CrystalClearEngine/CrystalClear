using System;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	[Serializable]
	[DataContract]
	public class ImaginaryPrimitive
		: ImaginaryObject
	{
		public ImaginaryPrimitive(object value)
			: base(value.GetType(), Array.Empty<ImaginaryObject>())
		{
			// Store the type for temporary use.
			Type valueType = value.GetType();

			// Does the type qualify as primitive?
			if (!QualifiesAsImaginaryPrimitive(valueType))
			{
				throw new ArgumentException($"The provided type does not qualify as an ImaginaryPrimitive. Type = {valueType.FullName}"); // TODO: use custom exception.
			}

			PrimitiveObjectValue = value;
		}

		// TODO make this and similar in other classes into extensions of type?
		public static bool QualifiesAsPrimitive(object valueToCheck)
			=> QualifiesAsImaginaryPrimitive(valueToCheck.GetType());

		// TODO make this and similar in other classes into extensions of type?
		public static bool QualifiesAsImaginaryPrimitive(Type type)
			=> type.IsPrimitive
				|| type == typeof(string)
				|| type.IsAssignableFrom(typeof(string))
				|| type.IsAssignableFrom(typeof(IFormattable))
				|| type.IsAssignableFrom(typeof(IConvertible)); // TODO somehow add some kind of IEditorSerializable to the mix here!

		public override object CreateInstance()
		{
			return PrimitiveObjectValue;
		}

		[DataMember]
		public object PrimitiveObjectValue;

		public string StringValue => Convert.ToString(PrimitiveObjectValue); // TODO make GetAsString method?

		public override string ToString()
		{
			return StringValue;
		}
	}
}
