using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

// TODO: might make redundant by storing name as part of ImaginaryHierarchyObjects?
namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[DataContract]
	public sealed class ImaginaryHierarchy : ImaginaryHierarchyObject
	{
		public ImaginaryHierarchy(ImaginaryHierarchyObject imaginaryHierarchyObject, string name)
		{
			if (imaginaryHierarchyObject.UsesConstructorParameters())
				this.ImaginaryConstructionParameters = imaginaryHierarchyObject.ImaginaryConstructionParameters;
			else
				this.EditorData = imaginaryHierarchyObject.EditorData;

			this.AttatchedScripts = imaginaryHierarchyObject.AttatchedScripts;
			this.LocalHierarchy = imaginaryHierarchyObject.LocalHierarchy;
			this.ConstructionTypeName = imaginaryHierarchyObject.ConstructionTypeName;
		}

		[DataMember]
		public string HierarchyName { get; private set; }

		public ImaginaryHierarchyObject GetHierarcyObject()
		{
			return (ImaginaryHierarchyObject)this;
		}
	}
}
