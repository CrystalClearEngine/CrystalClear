using CrystalClear.ECS;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CrystalClear.ScriptUtilities
{
	// TODO: Take parent objects into consideration for local and global positions (maybe a separate HierarhchyTransform2D even?)!
	public class Transform2D : DataAttribute
	{
		public event Action TransformChanged;

		private Vector2 position;
		public Vector2 Position
		{
			get => position;
			set
			{
				TransformChanged();
				PositionChanged(value, position);
				position = value;
			}
		}
		public event Action<Vector2, Vector2> PositionChanged;

		private float rotation;
		public float Rotation
		{
			get => rotation;
			set
			{
				TransformChanged();
				RotationChanged(value, rotation);
				rotation = value;
			}
		}
		public event Action<float, float> RotationChanged;

		private Vector2 scale;
		public Vector2 Scale
		{
			get => scale;
			set
			{
				TransformChanged();
				ScaleChanged(value, scale);
				scale = value;
			}
		}
		public event Action<Vector2, Vector2> ScaleChanged;
	}
}
