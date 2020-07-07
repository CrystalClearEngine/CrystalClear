using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.MessageSystem
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class OnReceiveMessageAttribute : Attribute
	{
		public Type MessageType { get; }

		public OnReceiveMessageAttribute(Type messageType)
		{
			MessageType = messageType;
		}
	}
}
