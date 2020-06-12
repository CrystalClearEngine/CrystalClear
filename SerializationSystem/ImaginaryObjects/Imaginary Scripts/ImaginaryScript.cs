﻿using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	/// <summary>
	/// Stores data for constructing a Script.
	/// </summary>
	[DataContract]
	public class ImaginaryScript : ImaginaryObject
	{
		public ImaginaryScript(ImaginaryObject imaginaryObjectBase)
		{
			ImaginaryObjectBase = imaginaryObjectBase;
		}

		private ImaginaryScript()
		{ }

		public HierarchyObject AttatchedTo;

		public ImaginaryObject ImaginaryObjectBase;

		/// <summary>
		/// Creates a Script instance using the construction data stored, as well as the optional attatchedTo HierarchyObject (required if the Script type is a HierarchyScript).
		/// </summary>
		/// <returns>The created Script instance.</returns>
		public override object CreateInstance()
		{
			Script script = (Script)ImaginaryObjectBase.CreateInstance();

			return script;
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
