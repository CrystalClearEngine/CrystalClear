using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;

namespace Benchmarking.Benchmarks.EventSystem
{
	[MemoryDiagnoser]
	public class SubscriptionBenchmarks
	{
		[OnStartEvent]
		public void OnStart()
		{
		}

		[Benchmark(Baseline = true)]
		public void Current()
		{
			CrystalClear.EventSystem.EventSystem.SubscribeEvents(GetType(), this);
		}

		[Benchmark]
		public void OrderedSubscription()
		{
			SubscribeEventsOrdered(GetType(), this);
		}

		private static void SubscribeEventsOrdered(Type typeToSubscribe, object instance)
		{
			IEnumerable<(MethodInfo method, SubscribeToAttribute)> methodsToSubscribe =
				from MethodInfo method in
					typeToSubscribe.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where method.GetCustomAttribute<SubscribeToAttribute>() is not null
				select (method, method.GetCustomAttribute<SubscribeToAttribute>());

			methodsToSubscribe = methodsToSubscribe.OrderBy(x => x.Item2.Order);

			foreach ((MethodInfo method, SubscribeToAttribute) methodToSubscribe in methodsToSubscribe)
				methodToSubscribe.Item2.ScriptEvent.Subscribe(methodToSubscribe.method, instance);
		}

		[Benchmark]
		public void UnorderedSubscription()
		{
			SubscribeEventsUnordered(GetType(), this);
		}

		private static void SubscribeEventsUnordered(Type typeToSubscribe, object instance)
		{
			foreach (MethodInfo method in typeToSubscribe.GetMethods(
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				var subscribeToAttribute = method.GetCustomAttribute<SubscribeToAttribute>();
				subscribeToAttribute?.ScriptEvent.Subscribe(method, instance);
			}
		}
	}
}