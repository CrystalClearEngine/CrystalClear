using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public ImaginaryHierarchyObject()
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
		[DataMember]
		public virtual Dictionary<string, ImaginaryScript> AttatchedScripts { get; set; } = new Dictionary<string, ImaginaryScript>();

		/// <summary>
		/// Does not matter unless this ImaginaryObject is root.
		/// </summary>
		private string rootName;

		public string Name
		{
			get
			{
				if (parent is null)
				{
					return rootName;
				}
				else
				{
					return Parent.LocalHierarchy.First((keyValuePair) => keyValuePair.Value == this).Key;
				}
			}

			set
			{
				if (parent is null)
				{
					rootName = value;
				}
				else
				{
					Parent.LocalHierarchy.Remove(Name);

					Parent.LocalHierarchy.Add(value, this);
				}
			}
		}

		public HierarchyObject HierarchyObjectParent;

		public ImaginaryHierarchyObject Parent
		{
			get => parent?.TryGetTargetExt();
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
				// TODO: make the api more consistent, AddScriptManually and AddChild take the name and value in different places!
				script.Value.AttatchedTo = hierarchyObject;
				hierarchyObject.AddScriptManually((Script)script.Value.CreateInstance(), script.Key);
			}

			hierarchyObject.SetUp((HierarchyObjectParent == null), HierarchyObjectParent);

			return hierarchyObject;
		}

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			ImaginaryObjectBase.WriteThis(writer);

			WriteStringDictionary(AttatchedScripts, writer);

			WriteStringDictionary(LocalHierarchy, writer);
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			ImaginaryObjectBase = ReadImaginaryObject(reader, out _);

			AttatchedScripts = ReadStringDictionary<ImaginaryScript>(reader);

			LocalHierarchy = ReadStringDictionary<ImaginaryHierarchyObject>(reader);
		}
	}
}
