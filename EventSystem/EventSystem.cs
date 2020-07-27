// TODO: compile without this define for the standalone DLLS.
#define Editor

using System;
using System.Linq;
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
			var methodsToSubscribe = from MethodInfo method in typeToSubscribe.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) where method.GetCustomAttribute<SubscribeToAttribute>() != null select (method, method.GetCustomAttribute<SubscribeToAttribute>());

			methodsToSubscribe = methodsToSubscribe.OrderBy(x => x.Item2.Order);

			foreach(var methodToSubscribe in methodsToSubscribe)
				methodToSubscribe.Item2.ScriptEvent.Subscribe(methodToSubscribe.method, instance);
		}

		public static void UnsubscribeEvents(Type typeToUnsubscribe, object instance)
		{
			foreach (MethodInfo method in typeToUnsubscribe.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				SubscribeToAttribute subscribeToAttribute = method.GetCustomAttribute<SubscribeToAttribute>();
				subscribeToAttribute?.ScriptEvent.Unsubscribe((ScriptEventHandler)method.CreateDelegate(typeof(ScriptEventHandler), instance));
			}
		}

		// TODO: make IsSubscribedMethod(out SubscribeToAttribute) method
		/// <summary>
		/// Subscribes all events a method has (if any).
		/// </summary>
		public static void SubscribeMethod(MethodInfo method, object instance)
		{
			SubscribeToAttribute subscribeToAttribute = method.GetCustomAttribute<SubscribeToAttribute>();
			if (!(subscribeToAttribute is null))
			{
#if Editor
				if (method.ContainsGenericParameters != subscribeToAttribute.EventType.ContainsGenericParameters)
				{
					throw new Exception($"The event method {method.Name} contains generic parameters while the event type does not.");
				}

				if (method.ReturnType != typeof(void))
				{
					throw new Exception($"The event method {method.Name} has a return type of {method.ReturnType.Name}, which is not void.");
				}
#endif
				subscribeToAttribute?.ScriptEvent.Subscribe(method, instance);
			}
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
				var methodsToSubscribe = from MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) where method.GetCustomAttribute<SubscribeToAttribute>() != null select (method, method.GetCustomAttribute<SubscribeToAttribute>());

				methodsToSubscribe = methodsToSubscribe.OrderBy(x => x.Item2.Order);

				foreach (var methodToSubscribe in methodsToSubscribe)
					methodToSubscribe.Item2.ScriptEvent.Subscribe(methodToSubscribe.method, null);
			}
		}
	}
}
