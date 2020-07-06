using System;
using System.Linq;
using System.Reflection;

namespace CrystalClear.MessageSystem
{
	public static class MessageSystem
	{
		public static void SendMessage(this object recipient, Message message)
		{
			var messageRecievers = (from MethodInfo method in recipient.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) where method.GetCustomAttribute<OnReceiveMessageAttribute>()?.MessageType == message.GetType() select method);

			var toCall = (from MethodInfo method in messageRecievers select method.CreateDelegate(message.DelegateType, recipient));

			foreach (Delegate item in toCall)
			{
				item.DynamicInvoke(message);
			}
		}

		public static void SendMessage(this Type recipient, Message message)
		{
			var messageRecievers = (from MethodInfo method in recipient.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) where method.GetCustomAttribute<OnReceiveMessageAttribute>()?.MessageType == message.GetType() select method);

			var toCall = (from MethodInfo method in messageRecievers select method.CreateDelegate(message.DelegateType));

			foreach (Delegate item in toCall)
			{
				item.DynamicInvoke(message);
			}
		}
	}
}
