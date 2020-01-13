using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// Base class for creating classes which help make editing object in-editor easier, as well as enable loading them at runtime.
	/// </summary>
	public abstract class EditorObject
	{
		public EditorObject(Type constructionType, object[] constructorParams)
		{
			ConstructionType = constructionType;
			ConstructorParams = constructorParams;
		}

		public EditorObject()
		{

		}

		public Type ConstructionType;
		public object[] ConstructorParams;

		public string TypeName => ConstructionType.AssemblyQualifiedName;

		public virtual void GetModifier()
		{
		}
	}

	public class EditorHierarchyObject
		: EditorObject
	{
		public EditorHierarchyObject(Type constructionType, object[] constructorParams)
		{
			ConstructionType = constructionType;
			ConstructorParams = constructorParams;
		}

		public Dictionary<string, EditorHierarchyObject> LocalHierarchy;
		public List<EditorScript> AttatchedScripts;
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

	public class EditorScript
		: EditorObject
	{
		public EditorScript(Type constructionType, object[] constructorParams)
		{
			ConstructionType = constructionType;
			ConstructorParams = constructorParams;
		}

		public Script CreateInstance(HierarchyObject attatchedTo)
		{
			Script instance = new Script(ConstructionType, ConstructorParams, attatchedTo);

			return instance;
		}
	}
}
