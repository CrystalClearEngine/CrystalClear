using System.Linq;
using System.Reflection;

namespace CrystalClear.MessageSystem
{
	public static class MessageSystem
	{
		public static void SendMessage(this object recipient, Message message)
		{
			var messageRecievers = (from MethodInfo method in recipient.GetType().GetMethods() where method.GetCustomAttribute<OnReceiveMessageAttribute>()?.MessageType == message.GetType() select method);

			var toCall = (from MethodInfo method in messageRecievers select method.CreateDelegate(message.DelegateType, recipient));
		}
	}
}
