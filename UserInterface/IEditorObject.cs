using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.UserInterface
{
	public interface IEditorObject
	{
		public void ModifierUI()
		{
			EditorGUI.Text("No custom Modifier UI has been made for this object.");
		}
	}
}
