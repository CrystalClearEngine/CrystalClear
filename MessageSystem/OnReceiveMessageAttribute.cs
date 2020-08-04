using System;

namespace CrystalClear.MessageSystem
{
	// TODO: add OnReceiveAnyMessageAttribute?
	[AttributeUsage(AttributeTargets.Method)]
	public class OnReceiveMessageAttribute : Attribute
	{
		public OnReceiveMessageAttribute(Type messageType)
		{
			MessageType = messageType;
		}

		public Type MessageType { get; }
	}
}