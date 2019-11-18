using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// Any class that implements this interface is a manager of hierarchyObjects in some way or is related to said task in some way
	/// </summary>
	public interface IHierarchyObjectManager
	{
		void Rename(object toRename, string newName);
		void Rename(string keyToRename, string newName);

		void Delete(object toDelete);
		void Delete(string keyToDelete);

		void Duplicate(object toDuplicate);
		void Duplicate(string keyToDuplicate);

		void Add(object toAdd, string name);

		string GetName(object toIdentify);

		IHierarchyObjectManager FollowPath(string path);

		Dictionary<string, IHierarchyObjectManager> SubHierarchy
		{
			get;
			set;
		}
	}
}
