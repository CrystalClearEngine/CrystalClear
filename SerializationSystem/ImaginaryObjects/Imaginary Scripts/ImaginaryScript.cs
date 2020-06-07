using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	/// <summary>
	/// Stores data for constructing a Script.
	/// </summary>
	[Serializable]
	[DataContract]
	public class ImaginaryScript
		: ImaginaryObject
	{ // TODO store the AttatchedTo ImaginaryHierarchyObject for this Script if it is a HierarchyScript. It doesn't serialize directly.
		public ImaginaryScript(Type constructionType, ImaginaryObject[] constructorParameters = null)
			: base(constructionType, constructorParameters)
		{

		}

		public ImaginaryScript(Type constructionType, EditorData editorData)
			: base(constructionType, editorData)
		{

		}

		/// <summary>
		/// Creates a Script instance using the construction data stored, as well as the optional attatchedTo HierarchyObject (required if the Script type is a HierarchyScript).
		/// </summary>
		/// <param name="attatchedTo">The HierarchyObject that this Script is attatched to IF it is a HierarchyScript.</param>
		/// <returns>The created Script instance.</returns>
		public Script CreateInstance(HierarchyObject attatchedTo = null)
		{
			object[] constructionParameters = new object[ImaginaryConstructionParameters.Length];
			for (int i = 0; i < ImaginaryConstructionParameters.Length; i++)
			{
				constructionParameters[i] = ImaginaryConstructionParameters[i]?.CreateInstance();
			}
			return new Script(GetConstructionType(), constructionParameters, attatchedTo);
		}
	}
}
