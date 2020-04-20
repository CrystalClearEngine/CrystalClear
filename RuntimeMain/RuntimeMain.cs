using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Diagnostics;
using System.Reflection;

namespace CrystalClear.RuntimeMain
{
	public static class RuntimeMain
	{
		public static bool IsRunning = false;

		public static void SubscribeAll(params Assembly[] userAssemblies)
		{
			EventSystem.EventSystem.FindAndSubscribeEventMethods(Assembly.GetAssembly(typeof(ScriptObject)));

			foreach (Assembly assembly in userAssemblies)
			{
				EventSystem.EventSystem.FindAndSubscribeEventMethods(assembly);
			}
		}

		public static void Run(string hierarchyPath, string hierarchyName)
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

			Run(hierarchyName, ImaginaryObjectSerialization.UnpackHierarchyFromFile(hierarchyPath));
		}

		public static void Run(string hierarchyName, ImaginaryHierarchyObject rootHierarchyObject)
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

			Run();
		}

		public static void Run()
		{
			if (IsRunning)
			{
				throw new Exception("Already running!");
			}

			IsRunning = true;

			// Raise the start event.
			StartEvent.Instance.RaiseEvent();
		}

		public static void Stop()
		{
			if (!IsRunning)
			{
				throw new Exception("Not running!");
			}

			IsRunning = false;
		}
	}
}
