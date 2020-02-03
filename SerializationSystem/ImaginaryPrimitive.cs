using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.SerializationSystem
{
	[DataContract]
	public class ImaginaryPrimitive
		: ImaginaryObject
	{
		public ImaginaryPrimitive(object value) : base(value.GetType(), Array.Empty<ImaginaryObject>())
		{
			Type type = value.GetType();

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

		internal string StringValue // TODO make GetAsString method?
		{
			get => Convert.ToString(PrimitiveObjectValue);
		}
	}
}
