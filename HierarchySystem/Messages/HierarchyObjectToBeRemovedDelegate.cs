using CrystalClear.MessageSystem;
using System;

namespace CrystalClear.HierarchySystem.Messages
{
	/// <summary>
	/// This message will be sent to any HierarchyObject that is going to be removed.
	/// </summary>
	public class HierarchyObjectToBeRemoved : Message
	{
		public override bool AllowStaticMethods => false;
	}
}
