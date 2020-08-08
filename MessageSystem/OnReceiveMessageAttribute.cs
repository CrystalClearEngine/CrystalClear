using System;

namespace CrystalClear.MessageSystem
{
	[AttributeUsage(AttributeTargets.Method)]
	public class OnReceiveMessageAttribute : Attribute
	{
		public OnReceiveMessageAttribute(Type messageType)
		{
			MessageType = messageType;
		}

		public bool ResolveWhetherToReceive(Type messageType)
		{
			if (ReceiveSubclasses)
			{
				return messageType.IsSubclassOf(MessageType) | MessageType == messageType;
			}
			else
			{
				return MessageType == messageType;
			}
		}
		
		public bool ReceiveSubclasses = true;

		public Type MessageType { get; }
	}
}