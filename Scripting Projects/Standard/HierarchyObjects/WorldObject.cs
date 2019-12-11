using CrystalClear.HierarchySystem;
using System.Collections.Generic;
using CrystalClear.ScriptUtilities;

namespace CrystalClear.Standard.HierarchyObjects
{
	/// <summary>
	/// A WorldObject is an HierarchyObject that has a Transform.
	/// </summary>
	public class WorldObject : HierarchyObject
	{
		/// <summary>
		/// Creates a WorldObject with the provided Transform.
		/// </summary>
		/// <param name="transform">The transfrom to use.</param>
		public WorldObject(Transform transform)
		{
			Transform = transform;
		}

		/// <summary>
		/// Creates a new WorldObject and creates a Transform with all Vectors initialized with the axis.
		/// </summary>
		/// <param name="axis">The dimension for the Transform.</param>
		public WorldObject(int axis)
		{
			Transform = new Transform(
				new Vector(axis),
				new Vector(axis),
				new Vector(axis));
		}

		/// <summary>
		/// Creates a WorldObject with a 3D Transform.
		/// </summary>
		public WorldObject()
		{
			Transform parent = (Parent as WorldObject)?.Transform;

			List<Transform> childTransforms = new List<Transform>();
			foreach (HierarchyObject child in LocalHierarchy.Values)
			{
				Transform transform = (child as WorldObject)?.Transform;
				if (transform != null)
				{
					childTransforms.Add(transform);
				}
			}

			Transform = new Transform(new Vector(3), new Vector(3), new Vector(3), parent, childTransforms.ToArray()); ;
		}

		public Transform Transform;

		protected override void OnLocalHierarchyChange()
		{
			// Initialize a list for temporarily containging the child transforms.
			List<Transform> childTransforms = new List<Transform>();
			// Iterate through the children in the localHierarchy.
			foreach (HierarchyObject child in LocalHierarchy.Values)
			{
				// Initialize a new Transform to store the Transform of the child (if any).
				Transform transform = (child as WorldObject)?.Transform;
				// Did the child have a Transform?
				if (transform != null)
				{
					// Add the child's Transform to the child transforms!
					childTransforms.Add(transform);
				}
			}
			// Set the child transforms to the temporary child Transform.
			Transform.Children = childTransforms.ToArray();
		}

		protected override void OnReparent(HierarchyObject newParent)
		{
			Transform.Parent = // Set the Transform's parent.
				(Parent as WorldObject)? // Get the parent as WorldObject or null.
				.Transform; // Get the parent's Transform.
		}
	}
}
