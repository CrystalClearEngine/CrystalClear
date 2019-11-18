using CrystalClear.HierarchySystem;
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
		public LARIS(Vector3 location, Quaternion rotation)
		{
			Location = location;
			Rotation = rotation;
		}

		public Vector3 Location { get; set; }
		public Quaternion Rotation { get; set; }
	}
}
