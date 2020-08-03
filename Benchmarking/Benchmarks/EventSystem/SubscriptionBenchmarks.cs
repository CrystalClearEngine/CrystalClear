using BenchmarkDotNet.Attributes;
using System;
using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;
using System.Reflection;

namespace Benchmarks.EventSystemBenchmarks
{
	[MemoryDiagnoser]
	public class SubscriptionOrderedVNonOrdered
	{
		[OnStartEvent]
		public void OnStart()
		{ }

		[Benchmark]
		public void OrderedSubscription()
		{
			EventSystem.SubscribeEvents(GetType(), this);
		}

		[Benchmark]
		public void UnorderedSubscription()
		{
			SubscribeEventsUnordered(GetType(), this);
		}

		private static void SubscribeEventsUnordered(Type typeToSubscribe, object instance)
		{
			foreach (MethodInfo method in typeToSubscribe.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				SubscribeToAttribute subscribeToAttribute = method.GetCustomAttribute<SubscribeToAttribute>();
				subscribeToAttribute?.ScriptEvent.Subscribe(method, instance);
			}
		}
	}
}
