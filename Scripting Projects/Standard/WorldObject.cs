using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CrystalClear.Standard.HierarchyObjects
{
	/// <summary>
	/// A WorldObject is an object that has a position, transform and display in the hierarchy.
	/// </summary>
	public class WorldObject : HierarchyObject
	{
		public LARIS LocationAndRotation;
	}

	/// <summary>
	/// Location And Rotation In Space
	/// </summary>
	public struct LARIS
	{
		//public Vector3 Location { get; set; } = new Vector3(0, 0, 0);
		//public Quaternion Rotation { get; set; } = new Quaternion();
	}
}
