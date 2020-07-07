using System;

namespace CrystalClear.MessageSystem
{
	public abstract class Message
	{
		// TODO: use flag enum instead.
		public virtual bool AllowStaticMethods => true;

		public virtual bool AllowInstanceMethods => true;

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
