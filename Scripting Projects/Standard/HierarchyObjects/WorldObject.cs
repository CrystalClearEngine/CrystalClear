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
			List<Transform> childTransforms = new List<Transform>();
			foreach (HierarchyObject child in LocalHierarchy.Values)
			{
				Transform transform = (child as WorldObject).Transform;
				if (transform != null)
				{
					childTransforms.Add(Transform);
				}
			}
			Transform.Children = childTransforms.ToArray();
		}

		protected override void OnReparent(HierarchyObject newParent)
		{
			Transform.Parent = // Set the transform's parent.
				(Parent as WorldObject)? // Get the parent as WorldObject or null.
				.Transform; // Get the parent's transform.
		}
	}

	public sealed class Transform
	{
		public Transform(Vector position, Vector rotation, Vector scale, Transform parent = null, Transform[] children = null)
		{
			globalPosition = position;
			globalRotation = rotation;
			globalScale = scale;
			Parent = parent;
			Children = children;
		}

		public Transform Parent;
		public Transform[] Children;

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

		public Vector LocalPosition => Parent.GlobalPosition - GlobalPosition;

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

		public Vector LocalRotation => Parent.GlobalRotation - GlobalRotation;

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
		/// Initializes a clone of a vector.
		/// </summary>
		/// <param name="vector">The vector to clone.</param>
		public Vector(Vector vector)
		{
			this = vector;
		}

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

		public static Vector operator / (Vector a, Vector b)
		{
			if (a.Axis.Length != b.Axis.Length)
			{
				throw new System.Exception("The axis of the two vectors are not comparable.");
			}

			Vector product = new Vector(a.Axis);

			for (int i = 0; i < product.Axis.Length; i++)
			{
				product.Axis[i] = a.Axis[i] / b.Axis[i];
			}

			return product;
		}

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
