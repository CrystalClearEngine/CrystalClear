using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

namespace Benchmarks
{
	public static class BenchmarkMain
	{
		static readonly Type[] benchmarks =
		{
			typeof(SubscriptionOrderedVNonOrdered),

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
