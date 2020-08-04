using System;
using System.Collections.Generic;

namespace CrystalClear.ScriptUtilities
{
	public struct Vector : IEquatable<Vector>
	{
		/// <summary>
		///     Initialize a Vector with the specified amount of axis.
		/// </summary>
		/// <param name="axis">The amount of axis, or the dimension.</param>
		public Vector(int axis)
		{
			Axis = new float[axis];
		}

		/// <summary>
		///     Initialize a Vector with the array of axis already provided.
		/// </summary>
		/// <param name="axis">The axis to use.</param>
		public Vector(params float[] axis)
		{
			Axis = axis;
		}

		/// <summary>
		///     Initializes a clone of a Vector.
		/// </summary>
		/// <param name="vector">The Vector to clone.</param>
		public Vector(Vector vector)
		{
			this = vector;
		}

		/// <summary>
		///     The length of Axis.
		/// </summary>
		public int AxisCount => Axis.Length;

		/// <summary>
		///     The axis of this Vector.
		/// </summary>
		public float[] Axis;

		/// <summary>
		///     The subtraction operator for the Vector. The difference is a new Vector with all it's axis equal to the difference
		///     of the corresponding axis of a - b.
		/// </summary>
		/// <param name="a">Vector a.</param>
		/// <param name="b">Vector b.</param>
		/// <returns>The difference Vector.</returns>
		public static Vector operator -(Vector a, Vector b)
		{
			if (a.AxisCount != b.AxisCount)
			{
				throw new Exception("The axis of the two vectors are not comparable.");
			}

			var difference = new Vector(a.AxisCount);

			for (var i = 0; i < a.AxisCount; i++)
			{
				difference.Axis[i] = a.Axis[i] - b.Axis[i];
			}

			return difference;
		}

		/// <summary>
		///     The addition operator for the Vector. The sum is a new Vector with all it's axis equal to the sum of the
		///     corresponding axis of a + b.
		/// </summary>
		/// <param name="a">Vector a.</param>
		/// <param name="b">Vector b.</param>
		/// <returns>The sum Vector.</returns>
		public static Vector operator +(Vector a, Vector b)
		{
			if (a.AxisCount != b.AxisCount)
			{
				throw new Exception("The axis of the two vectors are not comparable.");
			}

			var sum = new Vector(a.AxisCount);

			for (var i = 0; i < sum.AxisCount; i++)
			{
				sum.Axis[i] = a.Axis[i] + b.Axis[i];
			}

			return sum;
		}

		/// <summary>
		///     The division operator for the Vector. The quotient is a new Vector with all it's axis equal to the quotient of the
		///     corresponding axis of a / b.
		/// </summary>
		/// <param name="a">Vector a.</param>
		/// <param name="b">Vector b.</param>
		/// <returns>The quotient Vector.</returns>
		public static Vector operator /(Vector a, Vector b)
		{
			if (a.AxisCount != b.AxisCount)
			{
				throw new Exception("The axis of the two vectors are not comparable.");
			}

			var quotient = new Vector(a.AxisCount);

			for (var i = 0; i < quotient.AxisCount; i++)
			{
				quotient.Axis[i] = a.Axis[i] / b.Axis[i];
			}

			return quotient;
		}

		/// <summary>
		///     The multiplication operator for the Vector. The product is a new Vector with all it's axis equal to the product of
		///     the corresponding axis of a * b.
		/// </summary>
		/// <param name="a">Vector a.</param>
		/// <param name="b">Vector b.</param>
		/// <returns>The product Vector.</returns>
		public static Vector operator *(Vector a, Vector b)
		{
			if (a.AxisCount != b.AxisCount)
			{
				throw new Exception("The axis of the two vectors are not comparable.");
			}

			var product = new Vector(a.AxisCount);

			for (var i = 0; i < product.AxisCount; i++)
			{
				product.Axis[i] = a.Axis[i] * b.Axis[i];
			}

			return product;
		}

		/// <summary>
		///     The multiplication operator for the Vector. The product is a new Vector with all it's axis equal to the product of
		///     the corresponding axis of a * b.
		/// </summary>
		/// <param name="a">Vector a.</param>
		/// <param name="b">Vector b.</param>
		/// <returns>The product Vector.</returns>
		public static bool operator ==(Vector a, Vector b)
		{
			if (a.AxisCount != b.AxisCount)
			{
				throw new Exception("The axis of the two vectors are not comparable.");
			}

			var @true = true;

			for (var i = 0; i < a.AxisCount && @true; i++)
			{
				@true = a.Axis[i] == b.Axis[i];
			}

			return @true;
		}

		/// <summary>
		///     The multiplication operator for the Vector. The product is a new Vector with all it's axis equal to the product of
		///     the corresponding axis of a * b.
		/// </summary>
		/// <param name="a">Vector a.</param>
		/// <param name="b">Vector b.</param>
		/// <returns>The product Vector.</returns>
		public static bool operator !=(Vector a, Vector b) => !(a == b);

		public override bool Equals(object obj) => obj is Vector vector && vector == this;

		public bool Equals(Vector other) => other is Vector vector && vector == this;

		public override int GetHashCode() => 633581876 + EqualityComparer<float[]>.Default.GetHashCode(Axis);
	}
}