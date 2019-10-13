//Temporary, mostly for testing

using System.Numerics;

namespace CrystalClear.Scripting.ScriptingEngine.Scene
{
	/// <summary>
	///     Represents any item that is part of the hierarchy
	/// </summary>
	public abstract class HierarchyObject
	{
	}

	/// <summary>
	///     A WorldObject is an object that has a position, transform and display in the hierarchy.
	/// </summary>
	public class WorldObject : HierarchyObject
	{
		public Vector3 Location { get; set; } = new Vector3(0, 0, 0);
		public Quaternion Rotation { get; set; } = new Quaternion();
	}
}