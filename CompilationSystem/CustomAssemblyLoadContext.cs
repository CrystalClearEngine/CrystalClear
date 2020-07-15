using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;

namespace EditorMain
{
	public class CustomAssemblyLoadContext : AssemblyLoadContext
	{
		public CustomAssemblyLoadContext() : base(isCollectible: true)
		{

		}

		public void LoadAssembly(string path)
		{
			LoadFromAssemblyPath(path);
		}
	}
}
