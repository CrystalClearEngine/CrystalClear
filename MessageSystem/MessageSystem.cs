using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CrystalClear.MessageSystem
{
	public static class MessageSystem
	{
		// TODO: use DynamicMethods?
		static Dictionary<Type, Dictionary<Type, Delegate>> messageRecipientMethodsCache = new Dictionary<Type, Dictionary<Type, Delegate>>();

		public static void SendMessage<TMessage>(this object recipient, TMessage message, bool shouldThrow)
			where TMessage : Message
		{
			FindMessageRecipientMethods(recipient.GetType());

			if (messageRecipientMethodsCache.ContainsKey(recipient.GetType()))
			{
				if (messageRecipientMethodsCache[recipient.GetType()].ContainsKey(message.GetType()))
				{
					messageRecipientMethodsCache[recipient.GetType()][message.GetType()].DynamicInvoke(message);
					return;
				}

			}

		}

		/// <summary>Finds message recipient methods and fills in the cache.</summary>
		/// <returns>Whether any message recipient methods were found.</returns>
		public static bool FindMessageRecipientMethods(Type type)
		{
			if (messageRecipientMethodsCache.ContainsKey(type))
			{
				if (messageRecipientMethodsCache[type].Count > 0)
				{
					return true;
				}
			}

			bool hasMessageRecipientMethods = false;

			var dictionaryEntry = new KeyValuePair<Type, Dictionary<Type, Delegate>>(type, new Dictionary<Type, Delegate>());

			foreach (MethodInfo method in type.GetMethods())
			{
				var attribute = method.GetCustomAttribute<OnReceiveMessageAttribute>();
				if (!(attribute is null))
				{
					dictionaryEntry.Value.Add(attribute.MessageType, method.CreateDelegate());
				}
			}

			if (hasMessageRecipientMethods)
			{
				messageRecipientMethodsCache.Add(dictionaryEntry.Key, dictionaryEntry.Value);
			}

			return hasMessageRecipientMethods;
		}
	}
}
