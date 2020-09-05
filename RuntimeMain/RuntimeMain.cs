#define AllowDebug
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using static CrystalClear.ScriptUtilities.Utilities.ConsoleInput;
using System.Threading;

namespace CrystalClear.RuntimeMain
{
	public static class RuntimeMain
	{
		public static bool IsRunning;

		public static void Main(string[] args)
		{
			var assemblyPath = string.IsNullOrEmpty(args[0])
				? AskQuestion("Please enter a UserGeneratedCode to use.")
				: args[0];

#if AllowDebug
			bool isDebug = args.Contains("--debug");

			NamedPipeClientStream debugStream;

			if (isDebug)
			{
				Debugger.Launch();

				var pipeName = args[1];

				debugStream = new NamedPipeClientStream(pipeName);

				debugStream.Connect();

				//Console.WriteLine(ReadString(debugStream));
			}
#endif

			Assembly compiledAssembly = Assembly.LoadFrom(assemblyPath);

			if (compiledAssembly is null)
			{
				Output.ErrorLog($"error: the assembly at '{assemblyPath}' could not be loaded.", false);
				Environment.Exit(-2);
			}

			AppDomain.CurrentDomain.ProcessExit += (info, obj) => Stop();

			Run(new Assembly[] { compiledAssembly });

			// TODO: make the Main thread do something instead of wasting it.

			#region Exit handling

			ExitHandling:
			if (Console.ReadKey().Key == ConsoleKey.Escape)
			{
				// Exit on escape key.
				Stop();
				Environment.Exit(1);
			}

			goto ExitHandling;

			#endregion
		}

		public static void WaitForStop(int msCheckInterval)
		{
			while (IsRunning)
			{
				Thread.Sleep(msCheckInterval);
			}
		}

		private static void SubscribeAllStatic(params Assembly[] userAssemblies)
		{
			EventSystem.EventSystem.FindAndSubscribeStaticEventMethods(Assembly.GetAssembly(typeof(ScriptObject)));

			foreach (Assembly assembly in userAssemblies)
			{
				EventSystem.EventSystem.FindAndSubscribeStaticEventMethods(assembly);
			}
		}

		public static void RunWithImaginaryHierarchyObjectPath(Assembly[] userAssemblies, string hierarchyName,
			string hierarchyPath, bool raiseStartEvent = true)
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

			RunWithImaginaryHierarchyObject(userAssemblies, hierarchyName,
				(ImaginaryHierarchyObject) ImaginaryObjectSerialization.UnpackImaginaryObject(hierarchyPath),
				raiseStartEvent);
		}

		public static void RunWithImaginaryHierarchyObject(Assembly[] userAssemblies, string hierarchyName,
			ImaginaryHierarchyObject rootHierarchyObject, bool raiseStartEvent = true)
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

			#region Creating

			HierarchyManager.AddHierarchy(hierarchyName, (HierarchyObject) rootHierarchyObject.CreateInstance());

			#endregion

			Run(userAssemblies, raiseStartEvent);
		}

		public static void Run(Assembly[] userAssemblies, bool raiseStartEvent = true)
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

			SubscribeAllStatic(userAssemblies);

			IsRunning = true;

			if (raiseStartEvent)
			{
				// Raise the start event.
				StartEvent.Instance.RaiseEvent();
			}
		}

		// TODO: make this return a bool, so you can see if it was cancelled? (or maybe out a bool?)
		public static void Stop()
		{
			if (!IsRunning)
			{
				return;
			}

			StopEvent.Instance.RaiseEvent();

			IsRunning = false;
		}
	}
}