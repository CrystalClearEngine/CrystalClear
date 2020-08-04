using CrystalClear;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.MessageSystem;
using CrystalClear.ScriptUtilities;

namespace Scripts
{
	[IsScript]
	internal class Messager
	{
		[OnStartEvent]
		private void SendMessage()
		{
			var recipient = new Recipient();

			var myMessage = new MyMessage
			{
				Data = "MyData",
			};

			Output.Log("Sending message.");

			myMessage.SendTo(recipient);

			myMessage.SendTo(recipient.GetType());
		}
	}

	internal class Recipient
	{
		[OnReceiveMessage(typeof(MyMessage))]
		private void ReceiveMyMessage(MyMessage myMessage)
		{
			Output.Log("Received MyMessage!");
			Output.Log("Data: " + myMessage.Data);
		}

		[OnReceiveMessage(typeof(MyMessage))]
		private static void StaticReceiveMyMessage(MyMessage myMessage)
		{
			Output.Log("Received MyMessage in static method!");
			Output.Log("Data: " + myMessage.Data);
		}
	}

	internal class MyMessage : Message
	{
		public string Data;
	}
}