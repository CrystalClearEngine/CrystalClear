using CrystalClear.Scripting.Events.ScriptEvents;
using System.Reflection;

namespace CrystalClear.Scripting.ScriptingEngine
{
	public class Script
	{
		public Script(string code) //Combine compiling and findtypes etc into one!!
		{

		}

		public Assembly scriptAssembly;
		public IEvent[] events;
	}
}
