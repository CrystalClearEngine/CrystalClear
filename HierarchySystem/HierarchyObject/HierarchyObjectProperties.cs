using System.Linq;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// A HierarchyObject is an object that exists in a Hierarchy, can have child objects and which can have HierarchyScripts attached.
	/// </summary>
	public abstract partial class HierarchyObject
	// TODO: should probably not only rewrite some of these methods, but also make HierarchyObject immutable or something.
	// TODO: probably limit certain chars on names, such as \ / < > etc.
	{
		/// <summary>
		/// Is this HierarchyObject the root of the entire Hierarchy (has no parent)?
		/// </summary>
		public bool IsRoot => Parent is null;

		/// <summary>
		/// Checks whether or not this Hierarchy is in the LoadedHierarchies' list.
		/// </summary>
		public bool IsLoadedHierarchy
		{
			get
			{
				return HierarchyManager.LoadedHierarchies.Values.Contains(Root);
			}
		}

		/// <summary>
		/// Returns the root of this Hierarchy.
		/// </summary>
		public HierarchyObject Root
		{
			get
			{
				// Is this the root?
				if (IsRoot)
				{
					return this;
				}
				// If not, then we should refer to our parent. They should know :).
				else
				{
					return Parent.Root;
				}
			}
		}

		/// <summary>
		/// Returns the name of the containing hierarchy, or calls the HierarchyManager's rename method if set.
		/// </summary>
		public string HierarchyName
		{
			get => HierarchyManager.GetHierarchyName(Root);
			set
			{
				HierarchyManager.SetHierarchyName(Root, value);
			}
		}

		/// <summary>
		/// Returns the name of this HierarchyObject by looking it up via GetName() on the parent (or HierarchyManager if this HierarchyObject is the root). This property supports setting the name, which uses the SetName() method on the parent or HierarchyManager if this Hierarchy object is the root.
		/// </summary>
		public string Name
		{
			get
			{
				if (IsRoot)
				{
					return HierarchyManager.GetHierarchyName(this);
				}
				else
				{
					return Parent.GetChildName(this);
				}
			}
			set => Parent.SetChildName(this, value);
		}

		/// <summary>
		/// The entire current hierarchy from the root, for scripts' modifying pleasure.
		/// </summary>
		internal Hierarchy Hierarchy => Root.LocalHierarchy;

		public string Path // TODO: document this.
						   // DONE: use > insead of /
		{
			get
			{
				string path = Name;

				if (!IsRoot)
				{
					string parentPath = Parent.Path;
					path = path.Insert(0, parentPath + @">");
				}

				return path;
			}
		}
	}
}
