using System;
using System.IO;

namespace CrystalClear.SerializationSystem
{
	public class SerializationException : Exception
	{
		public readonly long ApproxPositionOfError;

		public SerializationException(BinaryWriter writer, string message) : this(writer, message, null)
		{
		}

		public SerializationException(BinaryWriter writer, string message, Exception innerException) : base(
			message + " Encountered at (approximately) position:" + writer.BaseStream.Position, innerException)
		{
			ApproxPositionOfError = writer.BaseStream.Position;
		}
	}

	public class DeserializationException : Exception
	{
		public readonly long ApproxPositionOfError;

		public DeserializationException(BinaryReader reader, string message) : this(reader, message, null)
		{
		}

		public DeserializationException(BinaryReader reader, string message, Exception innerException) : base(
			message + " Encountered at (approximately) position:" + reader.BaseStream.Position, innerException)
		{
			ApproxPositionOfError = reader.BaseStream.Position;
		}
	}
}