using CrystalClear;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.MessageSystem;
using CrystalClear.ScriptUtilities;
using System;

namespace Scripts
{
	[IsScript]
	class Messager
	{
		[OnStartEvent]
		void SendMessage()
		{
			Recipient recipient = new Recipient();

			MyMessage myMessage = new MyMessage()
			{
				Data = "MyData"
			};

			Output.Log("Sending message.");

			myMessage.SendTo(recipient);

			myMessage.SendTo(recipient.GetType());
		}
	}

	class Recipient
	{
		[OnReceiveMessage(typeof(MyMessage))]
		void ReceiveMyMessage(MyMessage myMessage)
		{
			Output.Log("Received MyMessage!");
			Output.Log("Data: " + myMessage.Data);
		}

		[OnReceiveMessage(typeof(MyMessage))]
		static void StaticReceiveMyMessage(MyMessage myMessage)
		{
			Output.Log("Received MyMessage in static method!");
			Output.Log("Data: " + myMessage.Data);
		}
	}

	class MyMessage : Message
	{
		public string Data;
	}
}
