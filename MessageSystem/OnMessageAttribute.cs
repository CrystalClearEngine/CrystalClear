using System;
using System.Collections.Generic;
using System.Text;

namespace MessageSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class OnMessageAttribute : Attribute
	{
		public Type MessageType;

		public OnMessageAttribute(Type messageType)
		{
			MessageType = messageType;
		}
	}
}
