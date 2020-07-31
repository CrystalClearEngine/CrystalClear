using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using static CrystalClear.ScriptUtilities.Utilities.ConsoleInput;

namespace CrystalClear.RuntimeMain
{
	public static class RuntimeMain
	{
		public static void Main(string[] args)
		{
			string assemblyPath = string.IsNullOrEmpty(args[0]) ? AskQuestion("Please enter a UserGeneratedCode to use.") : args[0];

			bool isDebug = args.Contains("--debug");

			NamedPipeClientStream debugStream;

			if (isDebug)
			{
				Debugger.Launch();

				string pipeName = args[1];

				debugStream = new NamedPipeClientStream(pipeName);

				debugStream.Connect();

				Console.WriteLine(ReadString(debugStream));
			}

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

			static string ReadString(PipeStream ioStream)
			{
				int len = 0;

				len = ioStream.ReadByte() * 256;
				len += ioStream.ReadByte();
				byte[] inBuffer = new byte[len];
				ioStream.Read(inBuffer, 0, len);

				return Encoding.ASCII.GetString(inBuffer);
			}

			static int WriteString(PipeStream ioStream, string outString)
			{
				byte[] outBuffer = Encoding.ASCII.GetBytes(outString);
				int len = outBuffer.Length;
				if (len > UInt16.MaxValue)
				{
					len = (int)UInt16.MaxValue;
				}
				ioStream.WriteByte((byte)(len / 256));
				ioStream.WriteByte((byte)(len & 255));
				ioStream.Write(outBuffer, 0, len);
				ioStream.Flush();

				return outBuffer.Length + 2;
			}
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
			HierarchyManager.AddHierarchy(hierarchyName, (HierarchyObject)rootHierarchyObject.CreateInstance());
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
