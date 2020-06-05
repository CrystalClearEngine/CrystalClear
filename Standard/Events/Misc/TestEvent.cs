using CrystalClear.EventSystem;
using System;

namespace CrystalClear.Standard.Events
{
	/// <summary>
	/// The test event event class. This event is used for testing purposes.
	/// </summary>
	public class TestEvent : ScriptEvent<TestEvent>
	{
		public override void RaiseEvent()
		{
			Console.WriteLine("Raising test event.");
			base.RaiseEvent();
			Console.WriteLine("Raised test event.");
		}
	}
}