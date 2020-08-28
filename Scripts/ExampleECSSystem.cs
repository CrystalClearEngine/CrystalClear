using CrystalClear.ECS;
using CrystalClear.Standard;
using CrystalClear.Standard.Events;
using System;
using System.Numerics;

namespace Scripts
{
	[AddToECSSystem(typeof(StaticMoverECSSystem))]
	public class Movable : DataAttribute
	{
		public bool ShouldMove;
		public Vector3 MoveDirection;
	}

	[IsECSSystem(RequiredDataAttribute = typeof(Movable))]
	public static class StaticMoverECSSystem
	{
		public static ECSSystem<Movable> ECSSystem = new ECSSystem<Movable>();

		[OnFrameUpdate]
		public static void Update()
		{
			ECSSystem.ExecuteOnEach((id, data) =>
			{
				if (data.ShouldMove && WorldECSSystem.ECSSystem.HasDataFor(id))
					WorldECSSystem.ECSSystem[id].Position += data.MoveDirection * (float)FrameUpdateEvent.DeltaTimeInSeconds;
			});
		}
	}
}
