using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// Base class for creating classes which help make editing object in-editor easier, as well as enable loading them at runtime.
	/// </summary>
	[Serializable]
	[DataContract]
	public abstract class EditorObject
	{
		public EditorObject(Type constructionType, object[] constructorParams)
		{
			ConstructionType = constructionType;
			ConstructorParams = constructorParams ?? new object[] { };
		}

		public EditorObject()
		{

		}

		public Type ConstructionType;
		[DataMember]
		public object[] ConstructorParams;

		[DataMember]
		public string TypeName
		{
			get
			{
				return ConstructionType.AssemblyQualifiedName;
			}
			set
			{
				ConstructionType = Type.GetType(value);
			}
		}

		public virtual void GetModifier()
		{
		}
	}

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

	[Serializable]
	[DataContract]
	public class EditorScript
		: EditorObject
	{
		public EditorScript(Type constructionType, object[] constructorParams)
		{
			ConstructionType = constructionType;
			ConstructorParams = constructorParams ?? new object[] { };
		}

		public Script CreateInstance(HierarchyObject attatchedTo)
		{
			Script instance = new Script(ConstructionType, ConstructorParams, attatchedTo);

			return instance;
		}
	}
}
