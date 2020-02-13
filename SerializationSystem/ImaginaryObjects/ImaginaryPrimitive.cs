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
			Type _ = value.GetType();

			// Does the type qualify as primitive?
			if (QualifiesAsPrimitive(_))
			{
				throw new ArgumentException($"Value has to be primitive! (Or a specifically supported type.) Type = {_.FullName}"); // TODO create custom exception.
			}

			// HOW DID I FORGET THIS? AM I RARTED?
			PrimitiveObjectValue = value;
		}

		// TODO make this and similar in other classes into extensions of type?
		public static bool QualifiesAsPrimitive(object valueToCheck)
			=> QualifiesAsPrimitive(valueToCheck.GetType());

		// TODO make this and similar in other classes into extensions of type?
		public static bool QualifiesAsPrimitive(Type type)
			=> type.IsPrimitive
				|| type.IsAssignableFrom(typeof(IFormattable))
				|| type.IsAssignableFrom(typeof(IConvertible)); // TODO somehow add some kind of IEditorSerializable to the mix here!

		public override object CreateInstance()
		{
			return PrimitiveObjectValue;
		}

		[DataMember]
		public object PrimitiveObjectValue;

		public string StringValue => Convert.ToString(PrimitiveObjectValue); // TODO make GetAsString method?
	}
}
