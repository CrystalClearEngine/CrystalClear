using CrystalClear.HierarchySystem;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	[Serializable]
	[DataContract]
	public class ImaginaryHierarchyObject
		: ImaginaryObject
	{
		public ImaginaryHierarchyObject(ImaginaryHierarchyObject parent, Type constructionType, ImaginaryObject[] constructorParams) : base(constructionType, constructorParams)
		{
			Parent = parent;
		}

		[DataMember]
		public Dictionary<string, ImaginaryHierarchyObject> LocalHierarchy = new Dictionary<string, ImaginaryHierarchyObject>();
		[DataMember]
		public Dictionary<string, ImaginaryScript> AttatchedScripts = new Dictionary<string, ImaginaryScript>();

		public ImaginaryHierarchyObject Parent
		{
			get
			{
				parent.TryGetTarget(out ImaginaryHierarchyObject editorHierarchyObject);
				return editorHierarchyObject;
			}
			set
			{
				parent.SetTarget(value);
			}
		}
		private WeakReference<ImaginaryHierarchyObject> parent = new WeakReference<ImaginaryHierarchyObject>(null);

		public HierarchyObject CreateInstance(HierarchyObject parent)
		{
			HierarchyObject instance = (HierarchyObject)Activator.CreateInstance(GetConstructionType(), args: ConstructorParams);

			if (parent != null)
			{
				instance.SetUp(parent);
			}

			foreach (string editorHierarchyName in LocalHierarchy.Keys)
			{
				instance.LocalHierarchy.Add(editorHierarchyName, LocalHierarchy[editorHierarchyName].CreateInstance(instance));
			}

			foreach (ImaginaryScript editorScript in AttatchedScripts.Values)
			{
				instance.AddScriptManually(editorScript.CreateInstance(instance));
			}

			return instance;
		}
	}
}
