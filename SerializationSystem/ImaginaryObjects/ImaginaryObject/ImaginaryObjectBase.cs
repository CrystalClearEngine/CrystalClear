using System.IO;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[DataContract]
	public abstract partial class ImaginaryObject : IBinarySerializable
	{
		protected ImaginaryObject()
		{ }

		/// <summary>
		/// Shorthand for 'WriteImaginaryObject(this, writer, writeImaginaryObjectUniqueIdentifier)'.
		/// </summary>
		public void WriteThis(BinaryWriter writer, bool writeImaginaryObjectUniqueIdentifier = true) => WriteImaginaryObject(this, writer, writeImaginaryObjectUniqueIdentifier);

		public abstract object CreateInstance();

		protected abstract void WriteConstructionInfo(BinaryWriter writer);

		protected abstract void ReadConstructionInfo(BinaryReader reader);

		void IBinarySerializable.WriteConstructionInfo(BinaryWriter writer) => WriteConstructionInfo(writer);

		void IBinarySerializable.ReadConstructionInfo(BinaryReader reader) => ReadConstructionInfo(reader);
	}
}
