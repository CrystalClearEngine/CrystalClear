using System.IO;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public interface IBinarySerializable
	{
		void WriteConstructionInfo(BinaryWriter writer);

		void ReadConstructionInfo(BinaryReader reader);

		//public bool Corrupted { get; }
	}
}