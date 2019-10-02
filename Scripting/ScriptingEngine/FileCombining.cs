using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CrystalClear.Scripting.ScriptingEngine
{
	/// <summary>
	/// Combines ALL user written code into one large file so that the compiler can compile it all at once and we dont have to worry about linking. If it works it aint stupid!
	/// </summary>
	[Obsolete("The compiling class no longer neccessitates this to cobine the files, go use that instead!", true)]
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
			string tempResult = Regex.Replace(result, "using [^; ]*;", "");

			string usings = string.Empty;
			foreach (var match in Regex.Matches(result, "using [^; ]*;"))
			{
				usings += "\n" + match.ToString();
			}
			result = tempResult;
			result = result.Insert(0, usings);


			return result;
		}
	}
}
