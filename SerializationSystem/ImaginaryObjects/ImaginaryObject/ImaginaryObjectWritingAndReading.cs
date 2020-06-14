using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public abstract partial class ImaginaryObject
	{
		private delegate ImaginaryObject CreateImaginaryObjectDelegate();

		private static ImaginaryObject ReadEmptyImaginaryObject(BinaryReader reader)
		{
			// TODO: use cache for the generated method.

			// Create a DynamicMethod that returns a new instance of the encoded type.
			Type imaginaryObjectType = Type.GetType(reader.ReadString());
			ConstructorInfo constructor = imaginaryObjectType.GetConstructor(Array.Empty<Type>());
			MethodInfo readConstructionInfoMethod = imaginaryObjectType.GetMethod("ReadConstructionInfo");
			MethodInfo createInstanceMethod = imaginaryObjectType.GetMethod("CreateInstance");

			DynamicMethod dynamicMethod = new DynamicMethod("CreateImaginaryObject", typeof(ImaginaryObject), Array.Empty<Type>());
			ILGenerator generator = dynamicMethod.GetILGenerator();

			generator.Emit(OpCodes.Newobj, constructor);
			generator.Emit(OpCodes.Ret);

			return ((CreateImaginaryObjectDelegate)dynamicMethod.CreateDelegate(typeof(CreateImaginaryObjectDelegate)))();
		}

		private static void WriteImaginaryObjectType(Type imaginaryObjectType, BinaryWriter writer)
		{
			writer.Write(imaginaryObjectType.AssemblyQualifiedName);
		}

		/// <summary>
		/// Reads an ImaginaryObject from the current position of the reader.
		/// </summary>
		internal static ImaginaryObject ReadImaginaryObject(BinaryReader reader, out bool? success)
		{
			int imaginaryObjectUniqueIdentifier = 0;

			bool usesImaginaryObjectUniqueIdentifier = reader.ReadBoolean();

			if (usesImaginaryObjectUniqueIdentifier)
				imaginaryObjectUniqueIdentifier = reader.ReadInt32();

			//try
			//{
			ImaginaryObject imaginaryObject = ReadEmptyImaginaryObject(reader);
			imaginaryObject.ReadConstructionInfo(reader);
			//}
			//catch (Exception)
			//{
			//	// Move the position forward to where the ImaginaryObject ends.
			//	return new CorruptedImaginaryObject();
			//}

			if (usesImaginaryObjectUniqueIdentifier)
				success = (imaginaryObjectUniqueIdentifier == reader.ReadInt32());
			else
				success = null;

			return imaginaryObject;
		}

		static private int totalWrittenImaginaryObjects = 0;
		
		internal static void WriteImaginaryObject(ImaginaryObject imaginaryObject, BinaryWriter writer, bool writeImaginaryObjectUniqueIdentifier = true)
		{
			writer.Write(writeImaginaryObjectUniqueIdentifier);

			int imaginaryObjectUniqueIdentifier = 0;
			if (writeImaginaryObjectUniqueIdentifier)
			{
				imaginaryObjectUniqueIdentifier = totalWrittenImaginaryObjects;
				imaginaryObjectUniqueIdentifier += imaginaryObject.GetHashCode();
				imaginaryObjectUniqueIdentifier += new Random().Next(imaginaryObjectUniqueIdentifier, int.MaxValue);

				writer.Write(imaginaryObjectUniqueIdentifier);
			}

			//try
			//{
			WriteImaginaryObjectType(imaginaryObject.GetType(), writer);

			imaginaryObject.WriteConstructionInfo(writer);
			//}
			//catch (Exception)
			//{
			//	// Revert position to where the writer was before.
			//	WriteImaginaryObject(new CorruptedImaginaryObject(), writer);
			//}

			if (writeImaginaryObjectUniqueIdentifier)
				writer.Write(imaginaryObjectUniqueIdentifier);

			totalWrittenImaginaryObjects++;
		}
	}
}
