using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrystalClear.MessageSystem
{

	public static class MessageSystem
	{
		// TODO: add out parameter messageGotReceived
		// TODO: add method where you can get the returns from the invokes?
		// TODO: use cache for messageReceivers and toCall?
		// TODO: use something else than DynamicInvoke
		// TODO: add send to multiple method.

		public static void SendMessage(this object recipient, Message message)
		{
			if (!message.AllowInstanceMethods)
				return;

			IEnumerable<MethodInfo> messageReceivers = from MethodInfo method in recipient.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) where method.GetCustomAttribute<OnReceiveMessageAttribute>()?.MessageType == message.GetType() select method;

			IEnumerable<Delegate> toCall = from MethodInfo method in messageReceivers select method.CreateDelegate(message.DelegateType, recipient);

			foreach (Delegate item in toCall)
			{
				// TODO: cast to Action<TMessage> or Action.MakeGeneric(message.GetType()) then invoke for better speed?
				item.DynamicInvoke(message);
			}
		}

		/// <summary>
		/// Sends a message to all static message recipient methods in the type.
		/// </summary>
		/// <param name="recipient">The recipient type that should receive the message.</param>
		/// <param name="message">The actual message to send.</param>
		public static void SendMessage(this Type recipient, Message message)
		{
			if (!message.AllowStaticMethods)
				return;

			IEnumerable<MethodInfo> messageReceivers = from MethodInfo method in recipient.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) where method.GetCustomAttribute<OnReceiveMessageAttribute>()?.MessageType == message.GetType() select method;

			IEnumerable<Delegate> toCall = from MethodInfo method in messageReceivers select method.CreateDelegate(message.DelegateType);

			foreach (Delegate item in toCall)
			{
				item.DynamicInvoke(message);
			}
		}
	}
}
