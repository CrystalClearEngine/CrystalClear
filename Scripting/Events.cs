using System;

namespace Scripting
{
	namespace Events
	{
		[AttributeUsage(AttributeTargets.Method)]
		public class EventAttribute : Attribute { }

		public class OnEventAttribute : Attribute
		{
			Type eventType;
			public OnEventAttribute(Type eventType)
			{
				this.eventType = eventType;
			}
		}

		public interface IEvent
		{
			/// <summary>
			/// Called whenever any event is called
			/// </summary>
			//void OnEvent();
		}

		public class OnStart : EventAttribute { }
		public interface IEStart : IEvent
		{
			/// <summary>
			/// Called at the start of the method
			/// </summary>
			void OnStart();
		}

		public class OnFrameUpdate : EventAttribute { }
		public interface IEFrameUpdate : IEvent
		{
			/// <summary>
			/// Called every time a new frame is drawn
			/// </summary>
			void OnFrameUpdate(float timeSinceLastFrame);
		}

		public class OnLowFPS : EventAttribute { }
		public interface IELowFPS : IEvent
		{
			/// <summary>
			/// Called once FPS reaches below a certain threshold. (Is forexample the standard event for rediscorvering to-be-culled dynamic objects in occulusion culling)
			/// </summary>
			/// <param name="FPS"> The framerate in frames per second the program is currently running at. </param>
			void OnLowFPS(int FPS);
		}
	}
}
