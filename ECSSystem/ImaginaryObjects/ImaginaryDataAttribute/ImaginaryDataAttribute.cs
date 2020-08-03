using CrystalClear.SerializationSystem.ImaginaryObjects;
using System;
using System.IO;

namespace CrystalClear.ECS.ImaginaryObjects.ImaginaryDataAttribute
{
	class ImaginaryDataAttribute : ImaginaryObject
	{
		public override object CreateInstance()
		{
			throw new NotImplementedException();
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
