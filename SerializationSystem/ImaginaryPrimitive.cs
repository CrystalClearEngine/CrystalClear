﻿using System;
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
			Type type = value.GetType();

			// Does the type qualify as primitive?
			if (QualifiesAsPrimitive(type))
			{
				throw new ArgumentException($"Value has to be primitive! (Or a specifically supported type.) Type = {type.FullName}"); // TODO create custom exception.
			}
		}

		// TODO make this and similar in other classes into extensions of type?
		public static bool QualifiesAsPrimitive(object valueToCheck)
		{
			return QualifiesAsPrimitive(valueToCheck.GetType());
		}

		// TODO make this and similar in other classes into extensions of type?
		public static bool QualifiesAsPrimitive(Type type)
		{
			return type.IsPrimitive || type.IsAssignableFrom(typeof(IFormattable)) || type.IsAssignableFrom(typeof(IConvertible)); // TODO somehow add some kind of IEditorSerializable to the mix here!
		}

		[DataMember]
		public object PrimitiveObjectValue;

		public string StringValue => Convert.ToString(PrimitiveObjectValue); // TODO make GetAsString method?
	}
}
