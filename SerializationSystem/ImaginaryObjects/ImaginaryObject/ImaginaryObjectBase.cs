using System.IO;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[DataContract]
	// TODO: should ImaginaryObject be generic, or at least optionally so, such as ImaginaryObject<TRealType> and CreateInstance returns TRealType?
	public abstract partial class ImaginaryObject : IBinarySerializable
	{
		void IBinarySerializable.WriteConstructionInfo(BinaryWriter writer)
		{
			WriteConstructionInfo(writer);
		}

		void IBinarySerializable.ReadConstructionInfo(BinaryReader reader)
		{
			ReadConstructionInfo(reader);
		}

		/// <summary>
		///     Shorthand for 'WriteImaginaryObject(this, writer, writeImaginaryObjectUniqueIdentifier)'.
		/// </summary>
		public void WriteThis(BinaryWriter writer, bool writeImaginaryObjectUniqueIdentifier = true)
		{
			WriteImaginaryObject(this, writer, writeImaginaryObjectUniqueIdentifier);
		}

		public abstract object CreateInstance();

		protected abstract void WriteConstructionInfo(BinaryWriter writer);

		protected abstract void ReadConstructionInfo(BinaryReader reader);
	}
}