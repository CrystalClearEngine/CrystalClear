using CrystalClear;
using CrystalClear.ECS;
using CrystalClear.Standard.Events;

public class MyCustomECSSystem : SelectiveECSSystem
{
	[OnFrameUpdate]
	public void OnFrameUpdate()
	{
		foreach (IEntity entity in EnumerateEntities())
		{
			Output.Log(entity.EntityId);
		}
	}
}