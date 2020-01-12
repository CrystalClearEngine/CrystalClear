﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// Base class for creating classes which help make editing object in-editor easier, as well as enable loading them at runtime.
	/// </summary>
	public abstract class EditorObject
	{
	}

	public abstract class RuntimeInstanciateableEditorObject
		:  EditorObject,
		IExtraObjectData
	{
		public Type ConstructionType;
		public object[] ConstructorParams;

		public virtual ExtraDataObject GetData()
		{
			return new ExtraDataObject()
			{
				{"TypeName", ConstructionType.AssemblyQualifiedName},
			};
		}

		public virtual void SetData(ExtraDataObject data)
		{
			ConstructionType = Type.GetType((string)data["TypeName"]);
		}
	}

	public class EditorHierarchyObject
		: RuntimeInstanciateableEditorObject
	{
		public Dictionary<string, EditorHierarchyObject> LocalHierarchy;
		public List<EditorScript> AttatchedScripts;

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
		: RuntimeInstanciateableEditorObject
	{
		public Script CreateInstance(HierarchyObject attatchedTo)
		{
			Script instance = new Script(ConstructionType, ConstructorParams, attatchedTo);

			return instance;
		}
	}
}
