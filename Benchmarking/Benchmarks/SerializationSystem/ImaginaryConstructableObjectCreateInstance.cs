using System.Reflection;
using BenchmarkDotNet.Attributes;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;

namespace Benchmarking.Benchmarks.SerializationSystem
{
	[MemoryDiagnoser]
	public class ImaginaryConstructableObjectCreateInstance
	{
		[GlobalSetup]
		public void Setup()
		{
			CrystalClear.RuntimeInformation.UserAssemblies = new[] { Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(HierarchyObject)), Assembly.GetAssembly(typeof(ScriptObject)) };
		}

		ImaginaryConstructableObject imaginaryConstructableObject = new ImaginaryConstructableObject(typeof(TestObject), new []{ new ImaginaryPrimitive("This is the data."), });
		
		[Benchmark]
		public object ImaginaryConstructableObjectCreateInstanceObjectReturn()
		{
			return imaginaryConstructableObject.CreateInstance();
		}

		[Benchmark(Baseline = true)]
		public TestObject ImaginaryConstructableObjectCreateInstanceCastReturn()
		{
			return (TestObject) imaginaryConstructableObject.CreateInstance();
		}
	}

	public class TestObject
	{
		public string data;
		
		public TestObject(string data)
		{
			this.data = data;
		}
	}
}