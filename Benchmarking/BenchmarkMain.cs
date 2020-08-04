using System;
using System.Reflection;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarks.EventSystemBenchmarks;
using Benchmarks.MessageSystemBenchmarks;

namespace Benchmarks
{
	public static class BenchmarkMain
	{
		public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(Assembly.GetEntryAssembly()).Run(args);
	}
}