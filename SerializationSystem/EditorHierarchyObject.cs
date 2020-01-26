using CrystalClear.HierarchySystem;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	[Serializable]
	[DataContract]
	public class EditorHierarchyObject
		: EditorObject
	{
		public EditorHierarchyObject(EditorHierarchyObject parent, Type constructionType, object[] constructorParams)
		{
			ConstructionType = constructionType;
			ConstructorParams = constructorParams ?? Array.Empty<object>();
			Parent = parent;
		}

		[DataMember]
		public Dictionary<string, EditorHierarchyObject> LocalHierarchy = new Dictionary<string, EditorHierarchyObject>();
		[DataMember]
		public Dictionary<string, EditorScript> AttatchedScripts = new Dictionary<string, EditorScript>();

		public EditorHierarchyObject Parent
		{
			get
			{
				parent.TryGetTarget(out EditorHierarchyObject editorHierarchyObject);
				return editorHierarchyObject;
			}
			set
			{
				parent.SetTarget(value);
			}
		}
		private WeakReference<EditorHierarchyObject> parent = new WeakReference<EditorHierarchyObject>(null);

		public HierarchyObject CreateInstance(HierarchyObject parent)
		{
			HierarchyObject instance = (HierarchyObject)Activator.CreateInstance(ConstructionType, args: ConstructorParams);

			if (parent != null)
			{
				instance.SetUp(parent);
			}

			foreach (string editorHierarchyName in LocalHierarchy.Keys)
			{
				instance.LocalHierarchy.Add(editorHierarchyName, LocalHierarchy[editorHierarchyName].CreateInstance(instance));
			}

			foreach (EditorScript editorScript in AttatchedScripts.Values)
			{
				instance.AddScriptManually(editorScript.CreateInstance(instance));
			}

			return instance;
		}
	}
}
