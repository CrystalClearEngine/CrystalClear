using System;
using System.IO;
using CrystalClear.SerializationSystem.ImaginaryObjects;

namespace CrystalClear.ECS.ImaginaryObjects.ImaginaryDataAttribute
{
	internal class ImaginaryDataAttribute : ImaginaryObject
	{
		public override object CreateInstance() => throw new NotImplementedException();

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