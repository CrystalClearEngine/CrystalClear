using System;

namespace CrystalClear.MessageSystem
{
	public abstract class Message
	{
		/// <summary>
		/// Whether to allow recipient methods to be static. Defaults to true.
		/// </summary>
		public virtual bool AllowStaticMethods => true;

		/// <summary>
		/// Whether to allow recipient methods to be instance methods. Defaults to true.
		/// </summary>
		public virtual bool AllowInstanceMethods => true;

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
