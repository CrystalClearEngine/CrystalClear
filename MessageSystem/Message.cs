using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CrystalClear.MessageSystem
{
	public abstract class Message
	{
		public abstract Type DelegateType { get; }

		public void SendTo(object recipient)
		{
			recipient.SendMessage(this);
		}

		public void SendTo(Type recipient)
		{
			recipient.SendMessage(this);
		}
	}
}
