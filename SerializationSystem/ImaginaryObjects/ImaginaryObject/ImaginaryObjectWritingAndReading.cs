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
		public static void WriteStringDictionary<TValue>(Dictionary<string, TValue> dictionary, BinaryWriter writer)
			where TValue : IBinarySerializable
		{
			writer.Write(dictionary.Count);

			foreach (KeyValuePair<string, TValue> attatchedScript in dictionary)
			{
				writer.Write(attatchedScript.Key);

				attatchedScript.Value.WriteConstructionInfo(writer);
			}
		}

		public static Dictionary<string, TValue> ReadStringDictionary<TValue>(BinaryReader reader)
			where TValue : ImaginaryObject
		{
			var dictionary = new Dictionary<string, TValue>();

			for (int i = 0; i < reader.ReadInt32(); i++)
			{
				dictionary.Add(reader.ReadString(), (TValue)ReadImaginaryObject(reader, out _));
			}

			return dictionary;
		}

		private delegate ImaginaryObject CreateImaginaryObjectDelegate();

		private static ImaginaryObject ReadEmptyImaginaryObject(BinaryReader reader)
		{
			// TODO: use cache for the generated method.

			// Create a DynamicMethod that returns a new instance of the encoded type.
			Type imaginaryObjectType = Type.GetType(reader.ReadString());
			ConstructorInfo constructor = imaginaryObjectType.GetConstructor(new Type[0]);

			if (constructor is null)
			{
				throw new Exception($"{imaginaryObjectType.FullName} does not have an empty constructor, which is required.");
			}

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
		public static ImaginaryObject ReadImaginaryObject(BinaryReader reader, out bool? success)
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

		private static int totalWrittenImaginaryObjects = 0;

		public static void WriteImaginaryObject(ImaginaryObject imaginaryObject, BinaryWriter writer, bool writeImaginaryObjectUniqueIdentifier = true)
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
