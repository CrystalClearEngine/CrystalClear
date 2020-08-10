using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Benchmarking.Benchmarks.Vector
{
	[MemoryDiagnoser]
	public class VectorMathsBenchmark
	{
		[Benchmark(Baseline = true)]
		public Vector3 Vector3Maths()
		{
			var vector3 = new Vector3();
			
			return vector3 += new Vector3(3, 1, 3);
		}
		
		[Benchmark]
		public CrystalClear.ScriptUtilities.Vector VectorMaths()
		{
			var vector = new CrystalClear.ScriptUtilities.Vector(3);
			
			return vector += new CrystalClear.ScriptUtilities.Vector(3, 1, 3);
		}
	}
}