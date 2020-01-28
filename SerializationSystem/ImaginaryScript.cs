using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	[Serializable]
	[DataContract]
	public class ImaginaryScript
		: ImaginaryObject
	{
		public ImaginaryScript(Type constructionType, ImaginaryObject[] constructorParams) : base(constructionType, constructorParams)
		{

		}

		public Script CreateInstance(HierarchyObject attatchedTo)
		{
			Script instance = new Script(GetConstructionType(), ConstructorParams, attatchedTo);

			return instance;
		}
	}
}
