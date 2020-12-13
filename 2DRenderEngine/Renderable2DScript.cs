using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.MessageSystem;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.WindowingSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.RenderEngine2D
{
	// TODO: move this stuff somewhere else, like standard and have standard reference to here.
	[IsScript]
	// TODO: maybe shouldn't depend, since it might reference a transform on a different object.
	[DependOnDataAttribute(typeof(Transform2D))]
	public class Renderable2DScript : HierarchyScript<HierarchyObject>
	{
		public Renderable2DScript(Transform2D renderTransform = null)
		{
			if (renderTransform is not null)
			{
				renderable2D.RenderTransform = renderTransform;
			}
			else
			{
				renderable2D.RenderTransform = HierarchyObject.GetAttribute<Transform2D>();
			}
		}

		private Renderable2D renderable2D;

		[OnFrameDraw]
		private void Draw()
		{
			//renderable2D.Sprite.Draw((RenderWindow)WindowingSystem.WindowingSystem.MainWindow, RenderStates.Default);
		}
	}
}
