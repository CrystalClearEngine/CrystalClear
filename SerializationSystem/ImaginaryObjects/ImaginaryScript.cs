using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// Stores data for constructing a Script.
	/// </summary>
	[Serializable]
	[DataContract]
	public class ImaginaryScript
		: ImaginaryObject
	{ // TODO store the AttatchedTo ImaginaryHierarchyObject for this Script if it is a HierarchyScript. It doesn't serialize directly.
		public ImaginaryScript(Type constructionType, ImaginaryObject[] constructorParameters)
			: base(constructionType, constructorParameters)
		{

		}

		/// <summary>
		/// Creates a Script instance using the construction data stored, as well as the optional attatchedTo HierarchyObject (required if the Script type is a HierarchyScript).
		/// </summary>
		/// <param name="attatchedTo">The HierarchyObject that this Script is attatched to IF it is a HierarchyScript.</param>
		/// <returns>The created Script instance.</returns>
		public Script CreateInstance(HierarchyObject attatchedTo = null)
		{
			return new Script(GetConstructionType(), ConstructionParameters, attatchedTo);
		}
	}
}
