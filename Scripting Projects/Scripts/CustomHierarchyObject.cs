using CrystalClear.HierarchySystem;

public class CustomHierarchyObject : HierarchyObject
{
	public CustomHierarchyObject()
	{

	}

	public CustomHierarchyObject(string textParameter)
	{
		Text = textParameter;
	}

	public string Text { get; }
}
