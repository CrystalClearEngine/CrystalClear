using System;
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
}
