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
		/// Subscribes all events in the method.
		/// </summary>
		public static void SubscribeMethod(MethodInfo method, object instance)
		{
			foreach (Attribute attribute in method.GetCustomAttributes())
			{
				if (attribute is SubscribeToAttribute subscribeToAttribute)
				{ // TODO prevent duplicate entries
					subscribeToAttribute.Event.Subscribe(method, instance);
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
				throw new Exception("Unequal array sizes - array lengths of classesToSubscribe and instances dont match");
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

		public static void FindAndSubscribeEventMethods(Assembly assembly) => FindAndSubscribeEventMethods(assembly.GetTypes());
		/// <summary>
		/// Finds and subscribes static event methods.
		/// </summary>
		/// <param name="types">The types to "scan" for static event methods.</param>
		public static void FindAndSubscribeEventMethods(Type[] types)
		{
			// Iterate through all provided types.
			foreach (Type type in types)
			{
				// Iterate through all methods in the type.
				foreach (MethodInfo method in type.GetMethods())
				{
					// Is the method static?
					if (method.IsStatic)
						// Let the event system handle the rest.
						EventSystem.SubscribeMethod(method, null);
				}
			}
		}
	}
}
