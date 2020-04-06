using CrystalClear.HierarchySystem;
using CrystalClear.Standard.Events;
using System;

public class CustomHierarchyObject : HierarchyObject
{
	public CustomHierarchyObject()
	{

	}

	public CustomHierarchyObject(string textParameter)
	{
		Text = textParameter;
	}

	public CustomHierarchyObject(string textParameter, bool pointlessBool)
	{
		Text = textParameter;
		PointlessBool = pointlessBool;
	}

	[OnStartEvent]
	public void PrintContents()
	{
		Console.WriteLine("HELLO!");
		Console.WriteLine(Text);
		Console.WriteLine(PointlessBool);
	}

	public string Text { get; }
	public bool PointlessBool { get; }
}
