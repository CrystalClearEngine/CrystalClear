using System;

namespace CrystalClear.ECS
{
	public sealed class IsECSSystemAttribute : Attribute
	{
		public Type RequiredDataAttribute;
	}
}
