using CrystalClear.MessageSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.HierarchySystem.Scripting.Messages
{
	/// <summary>
	/// This message will be sent to any script that is going to be removed.
	/// </summary>
	public class ScriptToBeRemoved : Message
	{
		public delegate void ScriptToBeRemovedDelegate(ScriptToBeRemoved message);

		public override bool AllowStaticMethods => false;

		public override Type DelegateType => typeof(ScriptToBeRemovedDelegate);
	}
}
