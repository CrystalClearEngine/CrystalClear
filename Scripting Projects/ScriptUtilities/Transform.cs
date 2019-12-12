using System.Collections.Generic;

namespace CrystalClear.ScriptUtilities
{
	/// <summary>
	/// A Transform is an object that keeps track of, and manages numerous functions related to the transformation and position, rotation and scaling properties of an object.
	/// </summary>
	public sealed class Transform
	{
		/// <summary>
		/// Constructs a Transform with the specified position, rotation, scale, parent and children.
		/// </summary>
		/// <param name="position">The global position to use for this Transform.</param>
		/// <param name="rotation">The global rotation to use for this Transform.</param>
		/// <param name="scale">The global scale to use for this Transform.</param>
		/// <param name="parent">The parent to use for this Transform.</param>
		/// <param name="children">The children to use for this Transform.</param>
		public Transform(Vector position, Vector rotation, Vector scale, Transform parent = null, List<Transform> children = null)
		{
			// Set the global position.
			globalPosition = position;
			// Set the global rotation.
			globalRotation = rotation;
			// Set the global scale.
			globalScale = scale;
			// Set the parent.
			Parent = parent;
			// Set the children.
			Children = children;
		}

		/// <summary>
		/// Creates a new Transform with all Vectors initialized with the axis as axis count.
		/// </summary>
		/// <param name="axis">The dimension for the Transform.</param>
		public Transform(int axis)
		{
			globalPosition = new Vector(axis);
			globalRotation = new Vector(axis);
			globalScale = new Vector(axis);
		}

		public void TransformBy(Vector position, Vector rotation, Vector scale)
		{
			GlobalPosition += position;
			GlobalRotation += rotation;
			GlobalScale += scale;
		}

		/// <summary>
		/// Adds a child to this Transform's children.
		/// </summary>
		/// <param name="child">The child to add.</param>
		public void AddChildTransform(Transform child)
		{
			// Set the child's parent to this.
			child.Parent = this;
			// Add the child to Children.
			Children.Add(child);
		}

		/// <summary>
		/// Removes a child from this Transform's children.
		/// </summary>
		/// <param name="child">The child to remove.</param>
		public void RemoveChildTransform(Transform child)
		{
			// Remove the child's parent.
			child.Parent = null;
			// Remove the child from Children.
			Children.Remove(child);
		}

		/// <summary>
		/// Removes a child from this Transform's children.
		/// </summary>
		/// <param name="id">The index at which the child to remove is located.</param>
		public void RemoveChildTransform(int id)
		{
			Children.RemoveAt(id);
		}

		/// <summary>
		/// The parent of this Transform.
		/// </summary>
		public Transform Parent;
		/// <summary>
		/// The children of this Transform.
		/// </summary>
		public List<Transform> Children = new List<Transform>();

		/// <summary>
		/// The global position of this Transform.
		/// </summary>
		private Vector globalPosition;
		public Vector GlobalPosition
		{
			get => globalPosition;
			set
			{
				Vector change = globalPosition - value;

				foreach (Transform child in Children)
				{
					child.globalPosition -= change;
				}

				globalPosition = value;
			}
		}

		/// <summary>
		/// The position of this Transform relative to it's parent's.
		/// </summary>
		public Vector LocalPosition => Parent.GlobalPosition - GlobalPosition;

		/// <summary>
		/// The global rotation of this Transform.
		/// </summary>
		private Vector globalRotation;
		public Vector GlobalRotation
		{
			get => globalRotation;
			set
			{
				Vector change = globalRotation - value;

				foreach (Transform child in Children)
				{
					child.globalRotation += change;
				}

				globalRotation = value;
			}
		}

		/// <summary>
		/// The rotation of this Transform relative to it's parent's.
		/// </summary>
		public Vector LocalRotation => Parent.GlobalRotation - GlobalRotation;

		/// <summary>
		/// The global scale of this Transform.
		/// </summary>
		private Vector globalScale;
		public Vector GlobalScale
		{
			get => globalScale;
			set
			{
				Vector change = globalScale - value;

				foreach (Transform child in Children)
				{
					child.globalScale += change;
				}

				globalScale = value;
			}
		}

		/// <summary>
		/// The scale of this Transform relative to it's parent's.
		/// </summary>
		public Vector LocalScale => Parent.GlobalScale - GlobalScale;
	}
}
