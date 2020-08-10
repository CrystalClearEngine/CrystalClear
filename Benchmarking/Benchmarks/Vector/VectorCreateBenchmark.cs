using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Benchmarking.Benchmarks.Vector
{
	[MemoryDiagnoser]
	public class VectorCreateBenchmark
	{
		[Benchmark(Baseline = true)]
		public Vector3 Vector3Create()
		{
			return new Vector3();
		}
		
		[Benchmark]
		public CrystalClear.ScriptUtilities.Vector VectorCreate()
		{
			return new CrystalClear.ScriptUtilities.Vector(3);
		}
	}
}