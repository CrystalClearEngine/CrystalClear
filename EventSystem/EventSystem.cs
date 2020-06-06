using System;
using System.Reflection;

namespace CrystalClear.EventSystem
{
	public static class EventSystem
	{
		/// <summary>
		/// Subscribes all events in the type.
		/// </summary>
		public static void SubscribeEvents(Type typeToSubscribe, object instance)
		{
			foreach (MethodInfo method in typeToSubscribe.GetMethods())
			{
				SubscribeToAttribute subscribeToAttribute = method.GetCustomAttribute<SubscribeToAttribute>();
				subscribeToAttribute?.ScriptEvent.Subscribe(method, instance);
			}
		}

		public static void UnsubscribeEvents(Type typeToUnsubscribe, object instance)
		{
			foreach (MethodInfo method in typeToUnsubscribe.GetMethods())
			{
				SubscribeToAttribute subscribeToAttribute = method.GetCustomAttribute<SubscribeToAttribute>();
				subscribeToAttribute?.ScriptEvent.Unsubscribe((ScriptEventHandler)method.CreateDelegate(typeof(ScriptEventHandler), instance));
			}
		}

		/// <summary>
		/// Subscribes all events a method has (if any).
		/// </summary>
		public static void SubscribeMethod(MethodInfo method, object instance)
		{
			SubscribeToAttribute subscribeToAttribute = method.GetCustomAttribute<SubscribeToAttribute>();
			subscribeToAttribute?.ScriptEvent.Subscribe(method, instance);
		}

		/// <summary>
		/// Finds and subscribes static event methods.
		/// </summary>
		/// <param name="assembly">The assembly to search for static event methods.</param>
		public static void FindAndSubscribeStaticEventMethods(Assembly assembly) => FindAndSubscribeStaticEventMethods(assembly.GetTypes());
		/// <summary>
		/// Finds and subscribes static event methods.
		/// </summary>
		/// <param name="types">The types to search for static event methods.</param>
		public static void FindAndSubscribeStaticEventMethods(Type[] types)
		{
			foreach (Type type in types)
			{
				foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
				{
					SubscribeMethod(method, null);
				}
			}
		}
	}
}
