using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scripting.ScriptingEngine
{
	/// <summary>
	/// Combines ALL user written code into one large file so that the compiler can compile it all at once.
	/// </summary>
	public static class FileCombining
	{
		public static string CombineFiles(List<string> files)
		{
			string result = string.Empty;
			foreach (string file in files)
			{
				result += file + "\n";
			}

			//We need to put all using directives at the top of the file. Therefore we will use this magic regex which finds them all: using [^; ]*;
			Regex.Matches(result, "using [^; ]*;");
			return null;
		}
	}
}
