using CrystalClear.HierarchySystem.Attributes;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	///     HierarchyRoot is a custom HierarchyObject type specifically designed to be at the root of a regular Hierarchy.
	/// </summary>
	[Hidden]
	public class HierarchyRoot : HierarchyObject
	{
		// The name of a HierarchyRoot is stored in the HierarchyManager in LoadedHierarchies.
		public new string Name => HierarchyManager.GetHierarchyName(this);

		// Refer HierarchyObjects to LocalHierarchy instead.
		public Hierarchy HierarchyObjects => LocalHierarchy;
#pragma warning disable IDE0051 // Remove unused private members
		// This has to be true, since a HierarchyRoot has to be at the root of the Hierarchy.
		private new bool IsRoot => true;

		// There can be no parent to a HierarchyRoot, as it is always at the root of the Hierarchy.
		public new HierarchyObject Parent => null;

		// This HierarchyObject has to be the root.
		private new HierarchyObject Root => this;
#pragma warning restore IDE0051 // Remove unused private members
	}
}