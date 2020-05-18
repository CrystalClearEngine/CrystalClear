using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem;
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
		public static void Main()
		{
			Assembly compiledAssembly = Assembly.LoadFile(AskQuestion("Enter the path to the UserGeneratedCode for the runtime to use"));

			SubscribeAll(compiledAssembly);

			Run();

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

		public static void SubscribeAll(params Assembly[] userAssemblies)
		{
			EventSystem.EventSystem.FindAndSubscribeEventMethods(Assembly.GetAssembly(typeof(ScriptObject)));

			foreach (Assembly assembly in userAssemblies)
			{
				EventSystem.EventSystem.FindAndSubscribeEventMethods(assembly);
			}
		}

		public static void Run(string hierarchyPath, string hierarchyName, bool raiseStartEvent = true)
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

			// TODO: create and use a method that unpacks ImaginaryHierarchies instead of straight up HierarchyObjects.
			Run(hierarchyName, ImaginaryObjectSerialization.UnpackHierarchyObjectFromFile(hierarchyPath), raiseStartEvent);
		}

		public static void Run(string hierarchyName, ImaginaryHierarchyObject rootHierarchyObject, bool raiseStartEvent = true)
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
			HierarchyManager.AddHierarchy(hierarchyName, rootHierarchyObject.CreateInstance(null));
			#region Profiling
			performanceStopwatchForCreate.Stop();
			Console.WriteLine(performanceStopwatchForCreate.ElapsedMilliseconds + " ms");
			#endregion Profiling
			#endregion

			Run(raiseStartEvent);
		}

		public static void Run(bool raiseStartEvent = true)
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

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
