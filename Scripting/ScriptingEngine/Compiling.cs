using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using CrystalClear.Scripting.Events.ScriptEvents;

namespace CrystalClear.Scripting.ScriptingEngine
{
	internal static class Compiling
	{
		public static Assembly CompileCode(string code)
		{
			// Create a code provider
			// This class implements the 'CodeDomProvider' class as its base. All of the current .Net languages (at least Microsoft ones)
			// come with thier own implemtation, thus you can allow the user to use the language of thier choice (though i recommend that
			// you don't allow the use of c++, which is too volatile for scripting use - memory leaks anyone?)
			using (Microsoft.CSharp.CSharpCodeProvider csProvider = new Microsoft.CSharp.CSharpCodeProvider())
			{

				// Setup our options!
				CompilerParameters options = new CompilerParameters
				{
					GenerateExecutable = false, // we want a Dll (or "Class Library" as its called in .Net)
					GenerateInMemory = true, // Saves us from deleting the Dll when we are done with it, though you could set this to false and save start-up time by next time by not having to re-compile
					IncludeDebugInformation = true,
					OutputAssembly = "Scripts",
					TempFiles = new TempFileCollection(Environment.CurrentDirectory, true),
				};
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
					foreach (var error in result.Errors)
					{
						Console.WriteLine(error);
					}
					return null;
				}

				return result.CompiledAssembly;
			}
		}

		public static Type[] FindInterfaceEvents(Assembly script)
		{
			List<Type> events = new List<Type>(); //temporary list

			if (script == null) //nullcheck
			{
				return null; //eye for an eye, null for a null
			}

			// Now that we have a compiled script, we will now get all types. This will let us add them to lists such as "scrips" etc.
			foreach (Type type in script.GetExportedTypes())
			{
				foreach (Type iface in type.GetInterfaces()) // Find all interfaces so that we can find for example event interfaces among them
				{
					if (iface == typeof(IEvent)) // This is an event interface
					{
						events.Add(iface);
					}
				}
			}

			return events.ToArray();
		}
	}
}