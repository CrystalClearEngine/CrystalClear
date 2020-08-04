using System.Diagnostics;
using CrystalClear.MessageSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Message = CrystalClear.MessageSystem.Message;

namespace UnitTests
{
	public class TestMessage : Message
	{
		
	}

	public class InstanceMessageRecipient
	{
		public int ReceivedMessages = 0;
		
		[OnReceiveMessage(typeof(TestMessage))]
		public void TestMessageReceiver()
		{
			ReceivedMessages++;
		}
		
		[OnReceiveMessage(typeof(TestMessage))]
		public void AnotherTestMessageReceiver(Message message)
		{
			ReceivedMessages++;
		}
		
		[OnReceiveMessage(typeof(TestMessage))]
		public void YetAnotherTestMessageReceiver(TestMessage message)
		{
			ReceivedMessages++;
		}
		
		[OnReceiveMessage(typeof(Message))]
		public void YetYetAnotherTestMessageReceiver(Message message)
		{
			ReceivedMessages++;
			
			Assert.IsInstanceOfType(message, typeof(TestMessage));
		}
		
		[OnReceiveMessage(typeof(Message), ReceiveSubclasses = false)]
		public void YetYetYetAnotherTestMessageReceiver(Message message)
		{
			ReceivedMessages++;
			
			Assert.Fail("This means that a subclass was received even though it was not supposed to be.");
		}
	}
	
	[TestClass]
	public class SendMessageTests
	{
		[TestMethod]
		public void InstanceSendMessageTest()
		{
			var recipient = new InstanceMessageRecipient();
			
			new TestMessage().SendTo(recipient);

			Debug.WriteLine(recipient.ReceivedMessages);
			Assert.IsTrue(recipient.ReceivedMessages == 4);
		}
	}
}