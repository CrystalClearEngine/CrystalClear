using CrystalClear.MessageSystem;
using System;

namespace CrystalClear.HierarchySystem.Messages
{
	/// <summary>
	/// This message will be sent to any HierarchyObject that is going to be removed.
	/// </summary>
	public class HierarchyObjectToBeRemoved : Message
	{
		public delegate void HierarchyObjectToBeRemovedDelegate(HierarchyObjectToBeRemoved message);

		public override bool AllowStaticMethods => false;

		public override Type DelegateType => typeof(HierarchyObjectToBeRemovedDelegate);
	}
}
