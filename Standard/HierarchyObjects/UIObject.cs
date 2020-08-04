using CrystalClear.HierarchySystem;

namespace CrystalClear.Standard.HierarchyObjects
{
	public enum MouseButton
	{
		LeftMouse = 1,
		MiddleMouse = 3,
		RightMouse = 2,
	}

	public class UIObject : HierarchyObject
	{
		public void Click(int mouseButtonID)
		{
			Click((MouseButton) mouseButtonID);
		}

		public void Click(MouseButton pressedButton)
		{
		}
	}
}