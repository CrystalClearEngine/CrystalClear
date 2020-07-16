using CrystalClear;
using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace UnitTests
{
	[TestClass]
	public class Initializer
	{
		[AssemblyInitialize]
		public static void Init(TestContext testContext)
		{
			CrystalClearInformation.UserAssemblies = new[]
			{
				Assembly.GetExecutingAssembly(),
				Assembly.GetAssembly(typeof(ScriptObject)),
				Assembly.GetAssembly(typeof(HierarchyObject)),
			};
		}
	}
}
