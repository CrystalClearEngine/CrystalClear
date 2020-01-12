using System;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// Internal type for use only to store data.
	/// </summary>
	[Serializable]
	[DataContract]
	internal struct ObjectStorage
	{
		[DataMember]
		internal string typeName;
		[DataMember]
		internal object[] constructorParameters;
		[DataMember]
		internal ExtraDataObject extraData;

		public ObjectStorage(Type objectType, object[] constructorParameters, ExtraDataObject extraData)
		{
			this.typeName = objectType.AssemblyQualifiedName;
			this.constructorParameters = constructorParameters;
			this.extraData = extraData;
		}

		public ObjectStorage(Type objectType, object[] constructorParameters, IExtraObjectData extraData)
			: this(objectType, constructorParameters, extraData?.GetData() ?? new ExtraDataObject())
		{ }
	}
}
