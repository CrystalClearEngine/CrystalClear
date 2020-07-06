using CrystalClear.MessageSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.HierarchySystem.Messages
{
	/// <summary>
	/// This message will be sent to any HierarchyObject that is going to be removed.
	/// </summary>
	public class HierarchyObjectToBeDeleted : Message
	{
		public delegate void ScriptToBeRemovedDelegate(HierarchyObjectToBeDeleted message);

		public override Type DelegateType => typeof(ScriptToBeRemovedDelegate);
	}
}
