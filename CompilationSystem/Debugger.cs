//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.IO;
//using System.CodeDom.Compiler;
//using System.CodeDom;
//using Microsoft.CSharp;

//namespace CrystalClear.CompilationSystem.Debugging
//{
//	public static class Debugger
//	{
//		public static void RuntimeDebuggerSetLine(/*[CallerLineNumber] */int? lineNumber = null, [CallerMemberName] string caller = null, [CallerFilePath] string file = null)
//		{
//			Output.Log($"Thread {Thread.CurrentThread.Name} is at line {lineNumber} in member {caller}, file {file}.");
//		}


//		// dissect the file, inject the debugging tools, put the file back together
//		public static string[] GenerateDebugFiles(string[] filePaths)
//		{
//			foreach (string path in filePaths)
//			{
//				string code = File.ReadAllText(path);

//				code.Insert(0, "using CrystalClear.CompilationSystem.Debugging;");

//				using (CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider())
//				{
//					CodeCompileUnit unit = cSharpCodeProvider.Parse(File.OpenText(path));
//					foreach (var item in unit.Namespaces)
//					{
//						item.
//					}
//				}

//				string[] lines = code.Split(
//					new[] { Environment.NewLine },
//					StringSplitOptions.None
//					);

//				for (int i = 0; i < lines.Length; i++)
//				{
//					string line = lines[i];

//					line.Insert(0, $"line{i}; RuntimeDebuggerSetLine({i});");
//				}
//			}
//		}
//	}
//}
