namespace CrystalClear.ScriptUtilities
{
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
