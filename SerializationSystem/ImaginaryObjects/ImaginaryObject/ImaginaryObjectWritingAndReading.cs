using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	// TODO: is all the data verification neccessary? The likelyhood of a bit-flip is low. Perhaps a checksum check is sufficent.
	public abstract partial class ImaginaryObject
	{
		private static int totalWrittenImaginaryObjects;

		public static void WriteStringDictionary<TValue>(Dictionary<string, TValue> dictionary, BinaryWriter writer)
			where TValue : IBinarySerializable
		{
			writer.Write(dictionary.Count);

			foreach (KeyValuePair<string, TValue> attachedScript in dictionary)
			{
				writer.Write(attachedScript.Key);

				attachedScript.Value.WriteConstructionInfo(writer);
			}
		}

		public static Dictionary<string, TValue> ReadStringDictionary<TValue>(BinaryReader reader)
			where TValue : ImaginaryObject
		{
			Dictionary<string, TValue> dictionary = new Dictionary<string, TValue>();

			for (var i = 0; i < reader.ReadInt32(); i++)
			{
				dictionary.Add(reader.ReadString(), (TValue) ReadImaginaryObject(reader, out _));
			}

			return dictionary;
		}

		private static ImaginaryObject ReadEmptyImaginaryObject(BinaryReader reader)
		{
			// TODO: use cache for the generated method.

			// Create a DynamicMethod that returns a new instance of the encoded type.
			var imaginaryObjectType = Type.GetType(reader.ReadString());
			ConstructorInfo constructor = imaginaryObjectType.GetConstructor(Array.Empty<Type>());

			if (constructor is null)
			{
				throw new Exception(
					$"{imaginaryObjectType.FullName} does not have an empty constructor, which is required.");
			}

			MethodInfo readConstructionInfoMethod = imaginaryObjectType.GetMethod("ReadConstructionInfo");
			MethodInfo createInstanceMethod = imaginaryObjectType.GetMethod("CreateInstance");

			var dynamicMethod =
				new DynamicMethod("CreateImaginaryObject", typeof(ImaginaryObject), Array.Empty<Type>());
			ILGenerator generator = dynamicMethod.GetILGenerator();

			generator.Emit(OpCodes.Newobj, constructor);
			generator.Emit(OpCodes.Ret);

			return ((CreateImaginaryObjectDelegate) dynamicMethod.CreateDelegate(
				typeof(CreateImaginaryObjectDelegate)))();
		}

		private static void WriteImaginaryObjectType(Type imaginaryObjectType, BinaryWriter writer)
		{
			writer.Write(imaginaryObjectType.AssemblyQualifiedName);
		}

		/// <summary>
		///     Reads an ImaginaryObject from the current position of the reader.
		/// </summary>
		public static ImaginaryObject ReadImaginaryObject(BinaryReader reader, out bool? success)
		{
			var imaginaryObjectUniqueIdentifier = 0;

			var usesImaginaryObjectUniqueIdentifier = reader.ReadBoolean();

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
				success = imaginaryObjectUniqueIdentifier == reader.ReadInt32();
			else
				success = null;

			return imaginaryObject;
		}

		public static void WriteImaginaryObject(ImaginaryObject imaginaryObject, BinaryWriter writer,
			bool writeImaginaryObjectUniqueIdentifier = true)
		{
			writer.Write(writeImaginaryObjectUniqueIdentifier);

			var imaginaryObjectUniqueIdentifier = 0;
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

		private delegate ImaginaryObject CreateImaginaryObjectDelegate();
	}
}