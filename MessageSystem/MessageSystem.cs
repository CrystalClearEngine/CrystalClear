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
		// TODO: add send to multiple method.
		// TODO: add SendMessageAsync method.

		public static void SendMessage(this object recipient, Message message)
		{
			if (!message.AllowInstanceMethods)
				return;
			
			foreach (MethodInfo method in recipient.GetType()
				.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				var attribute = method.GetCustomAttribute<OnReceiveMessageAttribute>();
				if (attribute is null) continue;
				if (attribute.ResolveWhetherToReceive(message.GetType()))
				{
					method.Invoke(recipient, method.GetParameters().Length == 0 ? null : new[] {message});
				}
			}
		}

		/// <summary>
		///     Sends a message to all static message recipient methods in the type.
		/// </summary>
		/// <param name="recipient">The recipient type that should receive the message.</param>
		/// <param name="message">The actual message to send.</param>
		public static void SendMessage(this Type recipient, Message message)
		{
			if (!message.AllowStaticMethods)
				return;

			foreach (MethodInfo method in recipient
				.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				var attribute = method.GetCustomAttribute<OnReceiveMessageAttribute>();
				if (attribute is null) continue;
				if (attribute.ResolveWhetherToReceive(message.GetType()))
				{
					method.Invoke(null, method.GetParameters().Length == 0 ? null : new[] {message});
				}
			}
		}
	}
}