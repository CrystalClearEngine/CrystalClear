using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem;
using System;

[Editable(nameof(Editor), nameof(Creator))]
//[Editable]
public class CustomHierarchyObject : HierarchyObject
{
	//[Editor]
	static void Editor(ref EditorData currentEditorData)
	{
		Console.WriteLine("CustomHierarchyObject editor opened.");
		if (currentEditorData["Text"] != null)
		{
			Console.WriteLine("Do you want to set a new value for Text?");
			if (!GetBool())
			{
				goto SetPointlessBool;
			}
		}
		Console.WriteLine("Choose a value for Text:");
		currentEditorData["Text"] = Console.ReadLine();
		SetPointlessBool:
		if (currentEditorData["PointlessBool"] != null)
		{
			Console.WriteLine("Do you want to set a new value for PointlessBool?");
			if (!GetBool())
			{
				goto Exit;
			}
		}
		Console.WriteLine("Choose true or false for Pointless bool:");
		currentEditorData["PointlessBool"] = GetBool().ToString();
		Exit:

		// TODO: equivalent to this should be part of the Editor UI system in the future.
		bool GetBool()
		{
			GetBool:
			switch (Console.ReadLine().ToLower())
			{
				case "y":
				case "yes":
				case "true":
				case "t":
				case "correct":
					return true;

				case "n":
				case "no":
				case "false":
				case "f":
				case "incorrect":
					return false;

				default:
					Console.WriteLine("Invalid input, retrying.");
					goto GetBool;
			}
		}
	}

	//[Creator]
	static object Creator(EditorData editorData)
	{
		CustomHierarchyObject createdCustomHierarchyObject = new CustomHierarchyObject
		{
			Text = editorData["Text"],

			PointlessBool = GetBool(editorData["PointlessBool"])
		};

		bool GetBool(string input)
		{
			switch (input.ToLower())
			{
				case "y":
				case "yes":
				case "true":
				case "t":
				case "correct":
					return true;

				case "n":
				case "no":
				case "false":
				case "f":
				case "incorrect":
					return false;

				default:
					throw new ArgumentException("input is not a bool!");
			}
		}

		return createdCustomHierarchyObject;
	}

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
		Console.WriteLine(Text);
		Console.WriteLine(PointlessBool);
	}

	public string Text { get; private set; }
	public bool PointlessBool { get; private set; }
}