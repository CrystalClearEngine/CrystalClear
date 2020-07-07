using CrystalClear.MessageSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.HierarchySystem.Messages
{
	/// <summary>
	/// This message will be sent to any HierarchyObject that is going to be removed.
	/// </summary>
	public class HierarchyObjectToBeRemoved : Message
	{
		public delegate void HierarchyObjectToBeRemovedDelegate(Messages.HierarchyObjectToBeRemoved message);

		public override Type DelegateType => typeof(HierarchyObjectToBeRemovedDelegate);
	}
}
