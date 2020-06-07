using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	public abstract class ImaginaryObjectBase<T>
	{
		public abstract T CreateInstance();

		internal abstract void WriteConstructionInfo(BinaryWriter writer, Encoding encoding);
	}
}
