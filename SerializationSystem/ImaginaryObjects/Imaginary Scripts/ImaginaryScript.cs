using CrystalClear.HierarchySystem;
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
		public HierarchyObject AttatchedTo;

		public ImaginaryObject ScriptBase;

		/// <summary>
		/// Creates a Script instance using the construction data stored, as well as the optional attatchedTo HierarchyObject (required if the Script type is a HierarchyScript).
		/// </summary>
		/// <param name="attatchedTo">The HierarchyObject that this Script is attatched to IF it is a HierarchyScript.</param>
		/// <returns>The created Script instance.</returns>
		public override object CreateInstance()
		{
			Script script = (Script)ScriptBase.CreateInstance();

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
