using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;

static class MainClass
{
	private static void Main(string[] args)
	{
		UIObject toBeClicked = HierarchySystem.LoadedHierarchies["UI"].HierarchyObjects["UIMap"].LocalHierarchy["StartButton"] as UIObject;
		toBeClicked.Click(0);
	}
}
