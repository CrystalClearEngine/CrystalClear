using System.Runtime.Serialization;

// TODO: use FileSystemWatcher to detect changes to the Prefab when in editor. Should probably prompt the user on startup if the file has moved/been deleted that they have to provide a new path.
// TODO: apply changes only when applied or some save event is called, or just update them (for now) on any change.
namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[DataContract]
	public sealed class HierarchyPrefab : ImaginaryHierarchyObject
	{
		public HierarchyPrefab(ImaginaryHierarchyObject imaginaryHierarchyObject, string name, string prefabPath)
		{
			ImaginaryObjectBase = imaginaryHierarchyObject.ImaginaryObjectBase;

			AttachedScripts = imaginaryHierarchyObject.AttachedScripts;
			LocalHierarchy = imaginaryHierarchyObject.LocalHierarchy;

			PrefabName = name;

			PrefabPath = prefabPath;
		}

		private HierarchyPrefab()
		{ }

		[DataMember]
		public string PrefabPath { get; set; }

		[DataMember]
		public string PrefabName { get; private set; }

		public void Apply()
		{
			ImaginaryObjectSerialization.SaveToFile(PrefabPath, this);
		}

		public void Revert()
		{
			HierarchyPrefab imaginaryObject = ImaginaryObjectSerialization.LoadFromSaveFile<HierarchyPrefab>(PrefabPath);

			ImaginaryObjectBase = imaginaryObject.ImaginaryObjectBase;

			AttachedScripts = imaginaryObject.AttachedScripts;
			LocalHierarchy = imaginaryObject.LocalHierarchy;

			PrefabName = imaginaryObject.PrefabName;
		}

		public ImaginaryHierarchyObject GetNonPrefab()
		{
			return (ImaginaryHierarchyObject)this;
		}
	}
}
