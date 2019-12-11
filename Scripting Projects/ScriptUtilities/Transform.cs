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
		public Transform(Vector position, Vector rotation, Vector scale, Transform parent = null, Transform[] children = null)
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
		/// The parent of this Transform.
		/// </summary>
		public Transform Parent;
		/// <summary>
		/// The children of this Transform.
		/// </summary>
		public Transform[] Children;

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
					child.globalPosition += change;
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
