using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Diagnostics;
using System.Reflection;
using static CrystalClear.ScriptUtilities.Utilities.ConsoleInput;

namespace CrystalClear.RuntimeMain
{
	public static class RuntimeMain
	{
		public static void Main(string[] args)
		{
			string assemblyPath = string.IsNullOrEmpty(args[0]) ? AskQuestion("Please enter a UserGeneratedCode to use.") : args[0];

			Assembly compiledAssembly = Assembly.LoadFrom(assemblyPath);

			if (compiledAssembly is null)
			{
				Output.ErrorLog($"error: the assembly at '{assemblyPath}' could not be loaded.", false);
				Environment.Exit(-2);
			}

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

		public static bool IsRunning = false;

		private static void SubscribeAllStatic(params Assembly[] userAssemblies)
		{
			EventSystem.EventSystem.FindAndSubscribeStaticEventMethods(Assembly.GetAssembly(typeof(ScriptObject)));

			foreach (Assembly assembly in userAssemblies)
			{
				EventSystem.EventSystem.FindAndSubscribeStaticEventMethods(assembly);
			}
		}

		public static void RunWithImaginaryHierarchyObjectPath(Assembly[] userAssemblies, string hierarchyName, string hierarchyPath, bool raiseStartEvent = true)
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

			RunWithImaginaryHierarchyObject(userAssemblies, hierarchyName, (ImaginaryHierarchyObject)ImaginaryObjectSerialization.UnpackImaginaryObject(hierarchyPath), raiseStartEvent);
		}

		public static void RunWithImaginaryHierarchyObject(Assembly[] userAssemblies, string hierarchyName, ImaginaryHierarchyObject rootHierarchyObject, bool raiseStartEvent = true)
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

			#region Creating
			#region Profiling
			Stopwatch performanceStopwatchForCreate = new Stopwatch();
			performanceStopwatchForCreate.Start();
			#endregion
			HierarchyManager.AddHierarchy(hierarchyName, (HierarchyObject)rootHierarchyObject.CreateInstance());
			#region Profiling
			performanceStopwatchForCreate.Stop();
			Output.Log(performanceStopwatchForCreate.ElapsedMilliseconds + " ms");
			#endregion Profiling
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
				throw new Exception("Not running!");
			}

			IsRunning = false;

			StopEvent.Instance.RaiseEvent();
		}
	}
}
