using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting
{
	namespace Events
	{
		[AttributeUsage(AttributeTargets.Method)]
		public class EventAttribute : Attribute { }

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
			void OnFrameUpdate();
		}
	}
}
