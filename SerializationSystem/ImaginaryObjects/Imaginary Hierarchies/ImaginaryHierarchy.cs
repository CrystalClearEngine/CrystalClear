using System.Runtime.Serialization;

// TODO: might make redundant by storing name as part of ImaginaryHierarchyObjects?
namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[DataContract]
	public sealed class ImaginaryHierarchy : ImaginaryHierarchyObject
	{
		public ImaginaryHierarchy(ImaginaryHierarchyObject imaginaryHierarchyObject, string name)
		{
			if (imaginaryHierarchyObject.UsesConstructorParameters())
				ImaginaryConstructionParameters = imaginaryHierarchyObject.ImaginaryConstructionParameters;
			else
				EditorData = imaginaryHierarchyObject.EditorData;

			AttatchedScripts = imaginaryHierarchyObject.AttatchedScripts;
			LocalHierarchy = imaginaryHierarchyObject.LocalHierarchy;
			ConstructionTypeName = imaginaryHierarchyObject.ConstructionTypeName;
		}

		[DataMember]
		public string HierarchyName { get; private set; }

		public ImaginaryHierarchyObject GetHierarchyObject()
		{
			return (ImaginaryHierarchyObject)this;
		}
	}
}
