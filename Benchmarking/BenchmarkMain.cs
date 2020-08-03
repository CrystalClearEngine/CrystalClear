﻿using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using Benchmarks.EventSystemBenchmarks;
using Benchmarks.MessageSystemBenchmarks;

namespace Benchmarks
{
	public static class BenchmarkMain
	{
		static readonly Type[] benchmarks =
		{
			typeof(SubscriptionOrderedVNonOrdered),
			typeof(LINQ_DynamicInvoke_vs_Invoke),
		};

		public static void Main()
		{
			Console.WriteLine("Running all benchmarks!");

			foreach (Type benchmark in benchmarks)
			{
				BenchmarkRunner.Run(benchmark, new DebugBuildConfig());
			}
		}
	}
}
