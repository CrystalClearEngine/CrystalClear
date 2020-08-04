using System;
using CrystalClear;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem;

[Editable(nameof(Editor), nameof(Creator))]
//[Editable]
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

	public string Text { get; private set; }

	public bool PointlessBool { get; private set; }

	//[Editor]
	private static void Editor(ref EditorData currentEditorData)
	{
		Output.Log("CustomHierarchyObject editor opened.");
		if (currentEditorData["Text"] != null)
		{
			Output.Log("Do you want to set a new value for Text?");
			if (!GetBool())
			{
				goto SetPointlessBool;
			}
		}

		Output.Log("Choose a value for Text:");
		currentEditorData["Text"] = Console.ReadLine();
		SetPointlessBool:
		if (currentEditorData["PointlessBool"] != null)
		{
			Output.Log("Do you want to set a new value for PointlessBool?");
			if (!GetBool())
			{
				goto Exit;
			}
		}

		Output.Log("Choose true or false for Pointless bool:");
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
					Output.ErrorLog("Invalid input, retrying.");
					goto GetBool;
			}
		}
	}

	//[Creator]
	private static object Creator(EditorData editorData)
	{
		var createdCustomHierarchyObject = new CustomHierarchyObject
		{
			Text = editorData["Text"],

			PointlessBool = GetBool(editorData["PointlessBool"]),
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

	[OnStartEvent]
	public void PrintContents()
	{
		Output.Log(Text);
		Output.Log(PointlessBool);
	}
}