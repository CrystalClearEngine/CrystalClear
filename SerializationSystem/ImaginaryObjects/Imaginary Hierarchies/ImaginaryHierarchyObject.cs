using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	/// <summary>
	/// ImaginaryHierarchyObject is an derivative of ImaginaryObject that is designed specifically for storing HierarchyObjects and the extra data that goes along with it.
	/// </summary>
	[DataContract]
	public class ImaginaryHierarchyObject : ImaginaryObject
	{
		protected ImaginaryHierarchyObject()
		{ }

		public ImaginaryObject HierarchyObjectBase;

		/// <summary>
		/// Contains the children of this ImaginaryHierarchyObject.
		/// </summary>
		[DataMember]
		public virtual Dictionary<string, ImaginaryHierarchyObject> LocalHierarchy { get; set; } = new Dictionary<string, ImaginaryHierarchyObject>();

		/// <summary>
		/// Contains the scripts which will be added to the HierarchyObject when an instance is created.
		/// </summary>
		[DataMember]
		public virtual Dictionary<string, ImaginaryScript> AttatchedScripts { get; set; } = new Dictionary<string, ImaginaryScript>();

		private HierarchyObject hierarchyObjectParent;

		public override object CreateInstance()
		{
			HierarchyObject hierarchyObject;

			hierarchyObject = (HierarchyObject)this.HierarchyObjectBase.CreateInstance();

			foreach (var child in LocalHierarchy)
			{
				child.Value.hierarchyObjectParent = hierarchyObject;
				hierarchyObject.AddChild(child.Key, (HierarchyObject)child.Value.CreateInstance());
			}

			foreach (var script in AttatchedScripts)
			{
				script.Value.AttatchedTo = hierarchyObject;
				hierarchyObject.AddScriptManually((Script)script.Value.CreateInstance(), script.Key);
			}

			hierarchyObject.SetUp(hierarchyObjectParent);

			return hierarchyObject;
		}

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			throw new NotImplementedException();
		}
	}
}
