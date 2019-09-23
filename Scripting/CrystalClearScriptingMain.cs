using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scripting
{
	public static class CrystalClearScriptingMain
	{
		static void Main()
		{
			//Hardcoded code to compile
			Assembly compiledScript = Compiling.CompileCode(
				"namespace SimpleScripts" +
				"{" +
				"    public class MyScriptMul5 : ScriptingInterface.IScriptType1" +
				"    {" +
				"        public string RunScript(int value)" +
				"        {" +
				"            return this.ToString() + \" just ran! Result: \" + (value*5).ToString();" +
				"        }" +
				"    }" +
				"    public class MyScriptNegate : ScriptingInterface.IScriptType1" +
				"    {" +
				"        public string RunScript(int value)" +
				"        {" +
				"            return this.ToString() + \" just ran! Result: \" + (-value).ToString();" +
				"        }" +
				"    }" +
				"}");

			if (compiledScript != null)
			{
				Compiling.RunScript(compiledScript);
			}
		}
	}
}
