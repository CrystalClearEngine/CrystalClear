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
				this.ConstructionParameters = imaginaryHierarchyObject.ConstructionParameters;
			else
				this.EditorData = imaginaryHierarchyObject.EditorData;

			this.AttatchedScripts = imaginaryHierarchyObject.AttatchedScripts;
			this.LocalHierarchy = imaginaryHierarchyObject.LocalHierarchy;
			this.ConstructionTypeName = imaginaryHierarchyObject.ConstructionTypeName;

			// Set the name.
			this.PrefabName = name;
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
				this.ConstructionParameters = prefab.ConstructionParameters;
			else
				this.EditorData = prefab.EditorData;

			this.AttatchedScripts = prefab.AttatchedScripts;
			this.LocalHierarchy = prefab.LocalHierarchy;
			this.ConstructionTypeName = prefab.ConstructionTypeName;
		}

		public ImaginaryHierarchyObject GetNonPrefab()
		{
			return (ImaginaryHierarchyObject)this;
		}
	}
}
