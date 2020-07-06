using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.MessageSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class OnReceiveMessageAttribute : Attribute
	{
		public Type MessageType;

		public OnReceiveMessageAttribute(Type messageType)
		{
			MessageType = messageType;
		}
	}
}
