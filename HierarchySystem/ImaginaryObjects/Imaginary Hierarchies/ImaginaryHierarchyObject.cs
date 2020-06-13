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
	[KnownType(typeof(ImaginaryObject)), KnownType(typeof(ImaginaryConstructableObject)), KnownType(typeof(ImaginaryEditableObject))]
	public class ImaginaryHierarchyObject : ImaginaryObject
	{
		protected ImaginaryHierarchyObject()
		{ }

		public ImaginaryHierarchyObject(ImaginaryHierarchyObject parent, ImaginaryObject imaginaryObjectBase)
		{
			Parent = parent;
			ImaginaryObjectBase = imaginaryObjectBase;
		}

		[DataMember]
		public ImaginaryObject ImaginaryObjectBase;

		/// <summary>
		/// Contains the children of this ImaginaryHierarchyObject.
		/// </summary>
		[DataMember]
		public virtual Dictionary<string, ImaginaryHierarchyObject> LocalHierarchy { get; set; } = new Dictionary<string, ImaginaryHierarchyObject>();

		/// <summary>
		/// Contains the scripts which will be added to the HierarchyObject when an instance is created.
		/// </summary>
		// TODO: add system where when a new script is added it will automatically be given its attached, maybe using [OnSerialized]? This for LocalHierarchy too.
		[DataMember]
		public virtual Dictionary<string, ImaginaryScript> AttatchedScripts { get; set; } = new Dictionary<string, ImaginaryScript>();

		// TODO: ensure this actually gets set.
		public HierarchyObject HierarchyObjectParent;

		// TODO: Going to need to be able to serialize this somehow, at least for the DataContractSerializer...
		public ImaginaryHierarchyObject Parent
		{
			get
			{
				ImaginaryHierarchyObject imaginaryHierarchyObject = null;

				parent?.TryGetTarget(out imaginaryHierarchyObject);

				return imaginaryHierarchyObject;
			}
			set
			{
				if (parent is null)
					parent = new WeakReference<ImaginaryHierarchyObject>(null);

				parent.SetTarget(value);
			}
		}

		private WeakReference<ImaginaryHierarchyObject> parent;

		[OnDeserialized]
		private void OnDeserialize(StreamingContext streamingContext)
		{
			foreach (ImaginaryHierarchyObject imaginaryHierarchyObject in LocalHierarchy.Values)
			{
				imaginaryHierarchyObject.Parent = this;
			}
		}

		public override object CreateInstance()
		{
			HierarchyObject hierarchyObject;

			hierarchyObject = (HierarchyObject)ImaginaryObjectBase.CreateInstance();

			foreach (var child in LocalHierarchy)
			{
				child.Value.HierarchyObjectParent = hierarchyObject;
				hierarchyObject.AddChild(child.Key, (HierarchyObject)child.Value.CreateInstance());
			}

			foreach (var script in AttatchedScripts)
			{
				script.Value.AttatchedTo = hierarchyObject;
				hierarchyObject.AddScriptManually((Script)script.Value.CreateInstance(), script.Key);
			}

			hierarchyObject.SetUp((HierarchyObjectParent == null), HierarchyObjectParent);

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
