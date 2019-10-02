using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using CrystalClear.Scripting.Events.ScriptEvents;

namespace CrystalClear.Scripting.ScriptingEngine
{
	internal static class Compiling
	{
		public static Assembly CompileCode(string[] fileNames)
		{
			using (Microsoft.CSharp.CSharpCodeProvider csProvider = new Microsoft.CSharp.CSharpCodeProvider())
			{
				CompilerParameters options = new CompilerParameters
				{
					GenerateExecutable = false,
					GenerateInMemory = true,
					IncludeDebugInformation = true,
					OutputAssembly = "Scripts",
					TempFiles = new TempFileCollection(Environment.CurrentDirectory, false),
				};

				options.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

				// Compile our code
				CompilerResults result;
				result = csProvider.CompileAssemblyFromFile(options, fileNames);

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