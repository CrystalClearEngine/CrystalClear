using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.EventSystem
{
	public static class EventSystem
	{
		/// <summary>
		/// Subscribes all events in the script.
		/// </summary>
		public static void SubscribeAllEvents(Type classToSubscribe, object instance)
		{
			foreach (MethodInfo method in classToSubscribe.GetMethods())
			{
				foreach (Attribute attribute in method.GetCustomAttributes())
				{
					if (attribute is SubscribeToAttribute subscribeToAttribute)
					{
						subscribeToAttribute.Event.Subscribe(method, instance);
					}
				}
			}
		}
	}
}
