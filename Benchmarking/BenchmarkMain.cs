using System;
using System.Reflection;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks.MessageSystemBenchmarks;

namespace Benchmarking
{
	public static class BenchmarkMain
	{
		public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(Assembly.GetEntryAssembly()).Run(args);
	}
}