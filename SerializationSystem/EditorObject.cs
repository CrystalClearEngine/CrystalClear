using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClear.HierarchySystem;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// Base class for creating classes which help make editing object in-editor easier, as well as enable loading them at runtime.
	/// </summary>
	public abstract class EditorObject
	{
	}

	public abstract class MultiTypeableEditorObject
		: EditorObject
	{
		public Type ObjectType;
	}

	public class EditorHierarchyObject
		: MultiTypeableEditorObject,
		IExtraObjectData
	{
		public Dictionary<string, EditorHierarchyObject> LocalHierarchy;
		public List<EditorScript> AttatchedScripts;
		public string Path;

		public ExtraDataObject GetData()
		{
			return new ExtraDataObject()
			{
				{"TypeName", ObjectType.AssemblyQualifiedName},
				{"Path", Path},
			};
		}

		public void SetData(ExtraDataObject data)
		{
			Path = (string)data["Path"];
			ObjectType = Type.GetType((string)data["TypeName"]);
		}
	}

	public class EditorScript
		: MultiTypeableEditorObject,
		IExtraObjectData
	{
		public ParameterEditorObject[] Parameters;

		ExtraDataObject IExtraObjectData.GetData()
		{
			return new ExtraDataObject()
			{
				{"TypeName", ObjectType.AssemblyQualifiedName},
			};
		}

		void IExtraObjectData.SetData(ExtraDataObject data)
		{
			ObjectType = Type.GetType((string)data["TypeName"]);
		}
	}

	public class ParameterEditorObject
		: MultiTypeableEditorObject,
		IExtraObjectData
	{
		// TODO Make sure that extra data is collected for this object as well etc.
		public object Value;

		ExtraDataObject IExtraObjectData.GetData()
		{
			return new ExtraDataObject()
			{
				{"TypeName", ObjectType.AssemblyQualifiedName},
				{"Value", Value},
			};
		}

		void IExtraObjectData.SetData(ExtraDataObject data)
		{
			ObjectType = Type.GetType((string)data["TypeName"]);
			Value = (string)data["Value"];
		}
	}
}
