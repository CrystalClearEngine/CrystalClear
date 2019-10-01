using System;
using System.Reflection;

namespace CrystalClear.Scripting.Events
{
	public static class Event
	{
		[Obsolete("Use Script.RunEvent or Event.RunEvent", false)]
		public static void RunStartEvents(Type[] events)
		{
			foreach (var item in events)
			{
				ConstructorInfo constructor = item.GetType().GetConstructor(Type.EmptyTypes);
				if (constructor != null && constructor.IsPublic)
				{
					if (constructor.Invoke(null) is ScriptEvents.IEStart scriptObject)
					{
						//lets run start
						try
						{
							scriptObject.OnStart();
						}
						catch (Exception e)
						{
							var trace = new System.Diagnostics.StackTrace(e, true);
							if (trace.FrameCount > 0)
							{
								var frame = trace.GetFrame(trace.FrameCount - 1);
								var className = frame.GetMethod().ReflectedType.Name;
								var methodName = frame.GetMethod().ToString();
								var lineNumber = frame.GetFileLineNumber();
								Console.WriteLine(className + methodName + lineNumber);
							}
						}
					}
				}
			}
		}
	}
		
	namespace ScriptEvents
	{
		[AttributeUsage(AttributeTargets.Method)]
		public abstract class EventAttribute : Attribute { }

		/// <summary>
		/// Empty interface only to be derived from
		/// </summary>
		public interface IEvent { }

		public sealed class OnStart : EventAttribute { }
		public interface IEStart : IEvent
		{
			/// <summary>
			/// Called at the start of the method
			/// </summary>
			void OnStart();
		}

		public sealed class OnFrameUpdate : EventAttribute { }
		public interface IEFrameUpdate : IEvent
		{
			/// <summary>
			/// Called every time a new frame is drawn
			/// </summary>
			void OnFrameUpdate(float timeSinceLastFrame);
		}

		public sealed class OnLowFPS : EventAttribute { }
		public interface IELowFPS : IEvent
		{
			/// <summary>
			/// Called once FPS reaches below a certain threshold. (is forexample the standard event for rediscorvering to-be-culled dynamic objects in occulusion culling)
			/// </summary>
			/// <param name="FPS"> The framerate in frames per second the program is currently running at </param>
			void OnLowFPS(int FPS);
		}

		public sealed class OnCompile : EventAttribute { }
		public interface IECompile : IEvent
		{
			/// <summary>
			/// Called once FPS goes below a certain threshold. (is forexample the standard event for rediscorvering to-be-culled dynamic objects in occulusion culling)
			/// </summary>
			/// <param name="FPS"> The framerate in frames per second the program is currently running at </param>
			void OnCompile(int FPS);
		}
	}
}