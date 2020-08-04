using CrystalClear.MessageSystem;

namespace CrystalClear.HierarchySystem.Scripting.Messages
{
	/// <summary>
	///     This message will be sent to any script that is going to be removed.
	/// </summary>
	public class ScriptToBeRemoved : Message
	{
		public override bool AllowStaticMethods => false;
	}
}