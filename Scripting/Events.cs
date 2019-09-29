﻿using System;

namespace Scripting
{
	namespace Events
	{
		[AttributeUsage(AttributeTargets.Method)]
		public abstract class EventAttribute : Attribute { }

		public class OnEventAttribute : Attribute
		{
			public Type eventType;
			public OnEventAttribute(Type eventType)
			{
				this.eventType = eventType;
			}
		}

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
