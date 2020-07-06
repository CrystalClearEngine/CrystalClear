using System.Linq;
using System.Reflection;

namespace CrystalClear.MessageSystem
{
	public static class MessageSystem
	{
		public static void SendMessage<TMessage>(this object recipient, TMessage message)
			where TMessage : Message
		{
			var messageRecievers = (from MethodInfo method in recipient.GetType().GetMethods() where method.GetCustomAttribute<OnReceiveMessageAttribute>()?.MessageType == typeof(TMessage) select method);

			var toCall = (from MethodInfo method in messageRecievers select method.CreateDelegate(message.DelegateType, recipient));
		}
	}
}
