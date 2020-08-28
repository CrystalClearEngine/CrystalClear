using System;

namespace Scripts
{
	public sealed class AddToECSSystemAttribute : Attribute
	{
		public readonly Type required;

		public AddToECSSystemAttribute(Type required)
		{
			this.required = required;
		}
	}
}