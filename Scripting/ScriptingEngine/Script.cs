using System;

namespace CrystalClear.Scripting.ScriptingEngine
{
	public class Script
	{
		public Script(Type scriptClass)
		{
			ScriptType = scriptClass;
		}

		/// <summary>
		/// The type that is the script class
		/// </summary>
		public Type ScriptType;
	}
}
