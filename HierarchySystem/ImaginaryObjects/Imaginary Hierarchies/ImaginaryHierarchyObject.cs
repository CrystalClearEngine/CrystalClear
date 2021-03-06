﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	/// <summary>
	///     ImaginaryHierarchyObject is an derivative of ImaginaryObject that is designed specifically for storing
	///     HierarchyObjects and the extra data that goes along with it.
	/// </summary>
	[DataContract]
	[KnownType(typeof(ImaginaryObject))]
	[KnownType(typeof(ImaginaryConstructableObject))]
	[KnownType(typeof(ImaginaryEditableObject))]
	public class ImaginaryHierarchyObject : ImaginaryObject
	{
		public HierarchyObject HierarchyObjectParent;

		[DataMember] public ImaginaryObject ImaginaryObjectBase;

		private WeakReference<ImaginaryHierarchyObject> parent;

		/// <summary>
		///     Does not matter unless this ImaginaryObject is root.
		/// </summary>
		private string rootName = "Root";

		public ImaginaryHierarchyObject()
		{
		}

		// TODO: rewrite, probably.
		public ImaginaryHierarchyObject(ImaginaryHierarchyObject parent, ImaginaryObject imaginaryObjectBase)
		{
			Parent = parent;
			ImaginaryObjectBase = imaginaryObjectBase;
		}

		/// <summary>
		///     Contains the children of this ImaginaryHierarchyObject.
		/// </summary>
		[DataMember]
		public virtual Dictionary<string, ImaginaryHierarchyObject> LocalHierarchy { get; set; } =
			new Dictionary<string, ImaginaryHierarchyObject>();

		/// <summary>
		///     Contains the scripts which will be added to the HierarchyObject when an instance is created.
		/// </summary>
		[DataMember]
		public virtual Dictionary<string, ImaginaryScript> AttachedScripts { get; set; } =
			new Dictionary<string, ImaginaryScript>();

		public string Name
		{
			get
			{
				if (!parent.TryGetTarget(out _))
				{
					return rootName;
				}

				return Parent.LocalHierarchy.First(keyValuePair => keyValuePair.Value == this).Key;
			}

			set
			{
				if (!parent.TryGetTarget(out _))
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

			hierarchyObject = (HierarchyObject) ImaginaryObjectBase.CreateInstance();

			foreach (KeyValuePair<string, ImaginaryHierarchyObject> child in LocalHierarchy)
			{
				child.Value.HierarchyObjectParent = hierarchyObject;
				hierarchyObject.AddChild(child.Key, (HierarchyObject) child.Value.CreateInstance());
			}

			foreach (KeyValuePair<string, ImaginaryScript> script in AttachedScripts)
			{
				// TODO: make the api more consistent, AddScriptManually and AddChild take the name and value in different places!
				script.Value.AttachedTo = hierarchyObject;
				hierarchyObject.AddScriptManually((Script) script.Value.CreateInstance(), script.Key);
			}

			hierarchyObject.SetUp(HierarchyObjectParent == null, HierarchyObjectParent);

			return hierarchyObject;
		}

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			ImaginaryObjectBase.WriteThis(writer);

			WriteStringDictionary(AttachedScripts, writer);

			WriteStringDictionary(LocalHierarchy, writer);
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			ImaginaryObjectBase = ReadImaginaryObject(reader, out _);

			AttachedScripts = ReadStringDictionary<ImaginaryScript>(reader);

			LocalHierarchy = ReadStringDictionary<ImaginaryHierarchyObject>(reader);
		}
	}
}