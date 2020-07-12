using System;

namespace CrystalClear.MessageSystem
{
	// TODO: add OnReceiveAnyMessageAttribute?
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
