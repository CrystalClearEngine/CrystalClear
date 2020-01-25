using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
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
