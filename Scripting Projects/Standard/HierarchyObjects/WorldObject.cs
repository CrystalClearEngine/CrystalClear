using CrystalClear.HierarchySystem;
using System.Collections.Generic;

namespace CrystalClear.Standard.HierarchyObjects
{
	/// <summary>
	/// A WorldObject is an HierarchyObject that has a transform.
	/// </summary>
	public class WorldObject : HierarchyObject
	{
		public WorldObject(Transform transform)
		{
			Transform = transform;
		}

		/// <summary>
		/// Creates a WorldObject with a 3D transform ().
		/// </summary>
		public WorldObject()
		{
			Transform parent = (Parent as WorldObject)?.Transform;

			List<Transform> childTransforms = new List<Transform>();
			foreach (HierarchyObject child in LocalHierarchy.Values)
			{
				Transform transform = (child as WorldObject).Transform;
				if (transform != null)
				{
					childTransforms.Add(Transform);
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
				// Initialize a new transform to store the transform of the child (if any).
				Transform transform = (child as WorldObject).Transform;
				// Did the child have a transform?
				if (transform != null)
				{
					// Add the child's transform to the child transforms!
					childTransforms.Add(Transform);
				}
			}
			// Set the child transforms to the temporary child transform.
			Transform.Children = childTransforms.ToArray();
		}

		protected override void OnReparent(HierarchyObject newParent)
		{
			Transform.Parent = // Set the transform's parent.
				(Parent as WorldObject)? // Get the parent as WorldObject or null.
				.Transform; // Get the parent's transform.
		}
	}

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

	public struct Vector
	{
		/// <summary>
		/// Initialize a Vector with the specified amount of axis.
		/// </summary>
		/// <param name="axis">The amount of axis, or the dimension.</param>
		public Vector(int axis)
		{
			Axis = new float[axis];
		}

		/// <summary>
		/// Initialize a Vector with the array of axis already provided.
		/// </summary>
		/// <param name="axis">The axis to use.</param>
		public Vector(params float[] axis)
		{
			Axis = axis;
		}

		/// <summary>
		/// Initializes a clone of a Vector.
		/// </summary>
		/// <param name="vector">The Vector to clone.</param>
		public Vector(Vector vector)
		{
			this = vector;
		}

		/// <summary>
		/// The subtraction operator for the Vector. The difference is a new Vector with all it's axis equal to the difference of the corresponding axis of a - b.
		/// </summary>
		/// <param name="a">The first Vector.</param>
		/// <param name="b">The second Vector.</param>
		/// <returns>The difference Vector.</returns>
		public static Vector operator - (Vector a, Vector b)
		{
			if (a.Axis.Length != b.Axis.Length)
			{
				throw new System.Exception("The axis of the two vectors are not comparable.");
			}

			Vector difference = new Vector(a.Axis);

			for (int i = 0; i < difference.Axis.Length; i++)
			{
				difference.Axis[i] = a.Axis[i] - b.Axis[i];
			}

			return difference;
		}

		/// <summary>
		/// The addition operator for the Vector. The sum is a new Vector with all it's axis equal to the sum of the corresponding axis of a + b.
		/// </summary>
		/// <param name="a">The first Vector.</param>
		/// <param name="b">The second Vector.</param>
		/// <returns>The sum Vector.</returns>
		public static Vector operator + (Vector a, Vector b)
		{
			if (a.Axis.Length != b.Axis.Length)
			{
				throw new System.Exception("The axis of the two vectors are not comparable.");
			}

			Vector sum = new Vector(a.Axis);

			for (int i = 0; i < sum.Axis.Length; i++)
			{
				sum.Axis[i] = a.Axis[i] + b.Axis[i];
			}

			return sum;
		}

		/// <summary>
		/// The division operator for the Vector. The quotient is a new Vector with all it's axis equal to the quotient of the corresponding axis of a / b.
		/// </summary>
		/// <param name="a">The first Vector.</param>
		/// <param name="b">The second Vector.</param>
		/// <returns>The quotient Vector.</returns>
		public static Vector operator / (Vector a, Vector b)
		{
			if (a.Axis.Length != b.Axis.Length)
			{
				throw new System.Exception("The axis of the two vectors are not comparable.");
			}

			Vector quotient = new Vector(a.Axis);

			for (int i = 0; i < quotient.Axis.Length; i++)
			{
				quotient.Axis[i] = a.Axis[i] / b.Axis[i];
			}

			return quotient;
		}

		/// <summary>
		/// The multiplication operator for the Vector. The product is a new Vector with all it's axis equal to the product of the corresponding axis of a * b.
		/// </summary>
		/// <param name="a">The first Vector.</param>
		/// <param name="b">The second Vector.</param>
		/// <returns>The product Vector.</returns>
		public static Vector operator * (Vector a, Vector b)
		{
			if (a.Axis.Length != b.Axis.Length)
			{
				throw new System.Exception("The axis of the two vectors are not comparable.");
			}

			Vector product = new Vector(a.Axis);

			for (int i = 0; i < product.Axis.Length; i++)
			{
				product.Axis[i] = a.Axis[i] * b.Axis[i];
			}

			return product;
		}

		public float[] Axis;
	}
}
