using System.Runtime.Serialization;

// TODO: apply changes only when applied or some save event is called, or just update them (for now) on any change.
namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	//[Editable(MainMode: false)]
	[DataContract]
	public sealed class ImaginaryHierarchyPrefab : ImaginaryHierarchyObject
	{
		public ImaginaryHierarchyPrefab(ImaginaryHierarchyObject imaginaryHierarchyObject, string name)
		{
			// Clone the imaginaryHierarchyObject's properties.
			if (imaginaryHierarchyObject.UsesConstructorParameters())
				ImaginaryConstructionParameters = imaginaryHierarchyObject.ImaginaryConstructionParameters;
			else
				EditorData = imaginaryHierarchyObject.EditorData;

			AttatchedScripts = imaginaryHierarchyObject.AttatchedScripts;
			LocalHierarchy = imaginaryHierarchyObject.LocalHierarchy;
			ConstructionTypeName = imaginaryHierarchyObject.ConstructionTypeName;

			// Set the name.
			PrefabName = name;
		}

		public string PrefabPath { get; set; }

		[DataMember]
		public string PrefabName { get; private set; }

		public void Apply(bool saveAll)
		{
			ImaginaryObjectSerialization.SaveToFile(PrefabPath, this);
		}

		public void Revert()
		{
			ImaginaryHierarchyObject prefab = ImaginaryObjectSerialization.LoadFromSaveFile<ImaginaryHierarchyObject>(PrefabPath);

			if (prefab.UsesConstructorParameters())
				ImaginaryConstructionParameters = prefab.ImaginaryConstructionParameters;
			else
				EditorData = prefab.EditorData;

			AttatchedScripts = prefab.AttatchedScripts;
			LocalHierarchy = prefab.LocalHierarchy;
			ConstructionTypeName = prefab.ConstructionTypeName;
		}

		public ImaginaryHierarchyObject GetNonPrefab()
		{
			return (ImaginaryHierarchyObject)this;
		}
	}
}
