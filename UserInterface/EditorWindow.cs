using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.UserInterface
{
	public abstract class EditorWindow
	{
		public bool Enabled { get; set; } = true;

		public abstract string WindowTitle { get; set; }

		public void UI()
		{
			if (Enabled)
			{
				if(ImGuiNET.ImGui.Begin(WindowTitle))
				{
					UIImpl();

					ImGuiNET.ImGui.End();
				}
			}
		}

		protected abstract void UIImpl();
	}
}
