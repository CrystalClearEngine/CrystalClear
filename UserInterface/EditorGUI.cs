using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ImGuiNET;

namespace CrystalClear.UserInterface
{
	/// <summary>
	/// Contains methods for creating IMGUI interfaces in the editor, currently uses ImGui.Net for this.
	/// For more (but less safe) methods use the ImGuiNet namespace instead.
	/// </summary>
	public static class EditorGUI
	{
		/// <summary>
		/// Marks the start of another window. All UI calls after this will be drawn to this window, until the window is ended with EndEditorWindow.
		/// </summary>
		/// <example>
		/// EditorGUI.BeginEditorWindow("My Window")
		///		EditorGUI.Text("Second window.");
		/// EditorGUI.EndEditorWindow();
		/// EditorGUI.Text("First window.");
		/// </example>
		/// <param name="name">The name of the window. Needs to be unique.</param>
		/// <returns>TODO</returns>
		// TODO: look up what this returns
		// TODO: add support for flags.
		public static bool BeginEditorWindow(string name)
		{
			return ImGui.Begin(name);
		}

		/// <summary>
		/// Marks the end of the current window.
		/// </summary>
		public static void EndEditorWindow()
		{
			ImGui.End();
		}

		/// <summary>
		/// Draws a text element.
		/// </summary>
		// TODO: don't use Vector4, use some friendlier class, like Color
		public static void Text(string text, Vector4 color = new Vector4())
		{
			if (text is null)
			{
				throw new ArgumentNullException(nameof(text));
			}

			if (color == Vector4.Zero)
			{
				ImGui.Text(text);
			}
			else
			{
				ImGui.TextColored(color, text);
			}
		}

		/// <summary>
		/// The button returns true if clicked.
		/// </summary>
		/// <example>
		/// if(EditorGui.Button("Press Me!"))
		/// {
		///		EditorGui.Text("The button has been pressed.");
		/// }
		/// </example>
		/// <returns>Whether the button is clicked or not.</returns>
		public static bool Button(string labelText)
		{
			if (labelText is null)
			{
				throw new ArgumentNullException(nameof(labelText));
			}

			return ImGui.Button(labelText);
		}

		//public static bool EditorSelectable(IEditorObject selectable, string labelText)
		//{
			
		//}
	}
}
