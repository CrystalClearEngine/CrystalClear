using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public abstract class ImaginaryObject
	{
		private delegate ImaginaryObject CreateImaginaryObjectDelegate();

		public ImaginaryObject ReadEmptyImaginaryObject(BinaryReader reader, Encoding encoding)
		{
			// TODO: use cache for this.

			// Create a DynamicMethod that returns a new instance of the encoded type.
			Type imaginaryObjectType = Type.GetType(reader.ReadString());
			if (!imaginaryObjectType.IsSubclassOf(typeof(ImaginaryObject)))
			{
				throw new ArgumentException();
			}
			ConstructorInfo constructor = imaginaryObjectType.GetConstructor(new Type[] { });
			MethodInfo readConstructionInfoMethod = imaginaryObjectType.GetMethod("ReadConstructionInfo");
			MethodInfo createInstanceMethod = imaginaryObjectType.GetMethod("CreateInstance");

			DynamicMethod dynamicMethod = new DynamicMethod("CreateImaginaryObject", typeof(ImaginaryObject), Array.Empty<Type>() );
			ILGenerator generator = dynamicMethod.GetILGenerator();
			
			generator.Emit(OpCodes.Newobj, constructor);
			generator.Emit(OpCodes.Ret);
			
			return ((CreateImaginaryObjectDelegate)dynamicMethod.CreateDelegate(typeof(CreateImaginaryObjectDelegate)))();
		}

		public void WriteEmptyImaginaryObject<TImaginaryObject>(BinaryWriter writer, Encoding encoding)
			where TImaginaryObject : ImaginaryObject
		{
			writer.Write(typeof(TImaginaryObject).AssemblyQualifiedName);
		}

		public abstract object CreateInstance();

		internal abstract void WriteConstructionInfo(BinaryWriter writer, Encoding encoding);

		internal abstract void ReadConstructionInfo(BinaryReader reader, Encoding encoding);
	}
}
