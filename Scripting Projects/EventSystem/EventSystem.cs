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
		/// Subscribes all events in the type.
		/// </summary>
		public static void SubscribeEvents(Type classToSubscribe, object instance)
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

		/// <summary>
		/// Same as SubscribeEvents, but in bulk. Make sure that the array lenghts of classesToSubscribe and instances match.
		/// </summary>
		public static void SubscribeEvents(Type[] classesToSubscribe, object[] instances)
		{
			if (classesToSubscribe.Length != instances.Length)
			{
				throw new Exception("Incorrect array sizes - array lenghts of classesToSubscribe and instances dont match");
			}

			for (int i = 0; i < classesToSubscribe.Length; i++)
			{
				Type classToSubscribe = classesToSubscribe[i];
				foreach (MethodInfo method in classToSubscribe.GetMethods())
				{
					foreach (Attribute attribute in method.GetCustomAttributes())
					{
						if (attribute is SubscribeToAttribute subscribeToAttribute)
						{
							subscribeToAttribute.Event.Subscribe(method, instances[i]);
						}
					}
				}
			}
		}
	}
}
