using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CrystalClear.MessageSystem
{
	public static class MessageSystem
	{
		public static void SendMessage<TMessage>(this object recipient, TMessage message, bool shouldThrow)
			where TMessage : Message
		{

		}
	}
}
