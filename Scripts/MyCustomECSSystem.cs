using CrystalClear.ECS;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using System;
using System.Collections.Generic;
using System.Text;

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