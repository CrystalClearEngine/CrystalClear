﻿using System;
using System.Reflection;
using System.CodeDom.Compiler;

namespace ScriptingInterface
{
	public interface IScriptType1
	{
		string RunScript(int value);
	}
}

namespace ScriptingExample
{
	static class Program
	{
		/// 
		/// The main entry point for the application.
		/// 
		[STAThread]
		static void Main()
		{

			// Lets compile some code (I'm lazy, so I'll just hardcode it all, i'm sure you can work out how to read from a file/text box instead
			Assembly compiledScript = CompileCode(
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
				RunScript(compiledScript);
			}
		}

		static Assembly CompileCode(string code)
		{
			// Create a code provider
			// This class implements the 'CodeDomProvider' class as its base. All of the current .Net languages (at least Microsoft ones)
			// come with thier own implemtation, thus you can allow the user to use the language of thier choice (though i recommend that
			// you don't allow the use of c++, which is too volatile for scripting use - memory leaks anyone?)
			Microsoft.CSharp.CSharpCodeProvider csProvider = new Microsoft.CSharp.CSharpCodeProvider();

			// Setup our options
			CompilerParameters options = new CompilerParameters();
			options.GenerateExecutable = false; // we want a Dll (or "Class Library" as its called in .Net)
			options.GenerateInMemory = true; // Saves us from deleting the Dll when we are done with it, though you could set this to false and save start-up time by next time by not having to re-compile
											 // And set any others you want, there a quite a few, take some time to look through them all and decide which fit your application best!

			// Add any references you want the users to be able to access, be warned that giving them access to some classes can allow
			// harmful code to be written and executed. I recommend that you write your own Class library that is the only reference it allows
			// thus they can only do the things you want them to.
			// (though things like "System.Xml.dll" can be useful, just need to provide a way users can read a file to pass in to it)
			// Just to avoid bloatin this example to much, we will just add THIS program to its references, that way we don't need another
			// project to store the interfaces that both this class and the other uses. Just remember, this will expose ALL public classes to
			// the "script"
			options.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

			// Compile our code
			CompilerResults result;
			result = csProvider.CompileAssemblyFromSource(options, code);

			if (result.Errors.HasErrors)
			{
				// TODO: report back to the user that the script has errored
				return null;
			}

			if (result.Errors.HasWarnings)
			{
				// TODO: tell the user about the warnings, might want to prompt them if they want to continue
				// runnning the "script"
			}

			return result.CompiledAssembly;
		}

		static void RunScript(Assembly script)
		{
			// Now that we have a compiled script, lets run them
			foreach (Type type in script.GetExportedTypes())
			{
				foreach (Type iface in type.GetInterfaces())
				{
					if (iface == typeof(ScriptingInterface.IScriptType1))
					{
						// yay, we found a script interface, lets create it and run it!

						// Get the constructor for the current type
						// you can also specify what creation parameter types you want to pass to it,
						// so you could possibly pass in data it might need, or a class that it can use to query the host application
						ConstructorInfo constructor = type.GetConstructor(System.Type.EmptyTypes);
						if (constructor != null && constructor.IsPublic)
						{
							// lets be friendly and only do things legitimitely by only using valid constructors

							// we specified that we wanted a constructor that doesn't take parameters, so don't pass parameters
							ScriptingInterface.IScriptType1 scriptObject = constructor.Invoke(null) as ScriptingInterface.IScriptType1;
							if (scriptObject != null)
							{
								//Lets run our script and display its results
								Console.WriteLine(scriptObject.RunScript(50));
							}
							else
							{
								// hmmm, for some reason it didn't create the object
								// this shouldn't happen, as we have been doing checks all along, but we should
								// inform the user something bad has happened, and possibly request them to send
								// you the script so you can debug this problem
							}
						}
						else
						{
							// and even more friendly and explain that there was no valid constructor
							// found and thats why this script object wasn't run
						}
					}
				}
			}
		}
	}
}