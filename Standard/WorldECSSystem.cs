using CrystalClear.ECS;
using CrystalClear.Standard.Events;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CrystalClear.Standard
{
	public class TransformAttribute : DataAttribute
	{
		public Vector3 Position;
	}

	[IsECSSystem]
	public static class WorldECSSystem
	{
		public static ECSSystem<TransformAttribute> ECSSystem = new ECSSystem<TransformAttribute>();
	}
}
