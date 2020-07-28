using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace Benchmarks
{
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

		public static void SubscribeEventsUnordered(Type typeToSubscribe, object instance)
		{
			foreach (MethodInfo method in typeToSubscribe.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				SubscribeToAttribute subscribeToAttribute = method.GetCustomAttribute<SubscribeToAttribute>();
				subscribeToAttribute?.ScriptEvent.Subscribe(method, instance);
			}
		}
	}
}
