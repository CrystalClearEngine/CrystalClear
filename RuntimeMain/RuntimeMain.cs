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
			Run(hierarchyName, ImaginaryObjectSerialization.UnpackHierarchyFromFile(hierarchyPath));
		}

		public static void Run(string hierarchyName, ImaginaryHierarchyObject rootHierarchyObject)
		{
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
			// Raise the start event.
			StartEvent.Instance.RaiseEvent();
		}
	}
}
