using System;
using System.Reflection;
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
		protected EditorObject(Type constructionType, object[] constructorParams)
		{
			ConstructionType = constructionType;
			ConstructorParams = constructorParams ?? Array.Empty<object>();
		}

		protected EditorObject()
		{

		}

		private Type constructionType;
		public Type ConstructionType
		{
			get
			{
				if (constructionType == null)
				{ // The object was probably deserialized, hence why it's null.
					constructionType = Type.GetType(typeName, true);
				}
				return constructionType;
			}
			set
			{
				typeName = value.AssemblyQualifiedName;
				constructionType = value;
			}
		}

		[DataMember]
		public object[] ConstructorParams;

		[DataMember]
		private string typeName;

		public string TypeName
		{
			get
			{
				if (ConstructionType == null)
				{
					ConstructionType = Type.GetType(typeName);
				}

				return ConstructionType.AssemblyQualifiedName;
			}
			set
			{
				Type type = Type.GetType(value);
				typeName = type.AssemblyQualifiedName;
				ConstructionType = type;
			}
		}


		public virtual void GetModifier()
		{
		}
	}
}
