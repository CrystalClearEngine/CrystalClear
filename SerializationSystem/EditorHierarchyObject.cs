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
			ConstructorParams = constructorParams ?? new object[] { };
			Parent = parent;
		}

		[DataMember]
		public Dictionary<string, EditorHierarchyObject> LocalHierarchy = new Dictionary<string, EditorHierarchyObject>();
		[DataMember]
		public List<EditorScript> AttatchedScripts = new List<EditorScript>();
		public EditorHierarchyObject Parent;

		public HierarchyObject CreateInstance(HierarchyObject parent)
		{
			HierarchyObject instance = (HierarchyObject)Activator.CreateInstance(ConstructionType, args: ConstructorParams);

			instance.SetUp(parent);

			foreach (string editorHierarchyName in LocalHierarchy.Keys)
			{
				instance.LocalHierarchy.Add(editorHierarchyName, LocalHierarchy[editorHierarchyName].CreateInstance(instance));
			}

			foreach (EditorScript editorScript in AttatchedScripts)
			{
				instance.AddScriptManually(editorScript.CreateInstance(instance));
			}

			return instance;
		}
	}
}
