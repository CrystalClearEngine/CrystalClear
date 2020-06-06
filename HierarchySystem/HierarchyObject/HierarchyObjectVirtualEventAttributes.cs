namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObjectProperties
	{
		// Overrideable event methods.

		/// <summary>
		/// OnLocalHierarchyChange is called when the LocalHierarchy is modified.
		/// </summary>
		protected internal virtual void OnLocalHierarchyChange()
		{

		}

		/// <summary>
		/// OnReparent is called when the HierarchyObject's parent updates.
		/// </summary>
		/// <param name="newParent">The new parent.</param>
		protected internal virtual void OnReparent(HierarchyObjectProperties newParent)
		{

		}

		/// <summary>
		/// OnSetUp is called when the HierarchyObject's SetUp method is called.
		/// </summary>
		protected internal virtual void OnSetUp()
		{

		}
	}
}
