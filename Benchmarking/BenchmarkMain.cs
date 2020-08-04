using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks.EventSystemBenchmarks;
using Benchmarks.MessageSystemBenchmarks;

namespace Benchmarks
{
	public static class BenchmarkMain
	{
		private static readonly Type[] benchmarks =
		{
			typeof(SubscriptionBenchmarks),
			typeof(SendMessageBenchmarks),
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