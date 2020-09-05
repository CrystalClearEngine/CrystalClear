using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.MessageSystem;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.RenderEngine2D
{
	[IsScript]
	[DependOnDataAttribute(typeof(Transform2D))]
	public class Renderable2DScript : HierarchyScript<HierarchyObject>
	{
		public Renderable2DScript(Transform2D renderTransform = null)
		{
			if (renderTransform != null)
			{
				renderable2D.RenderTransform = renderTransform;
			}
			else
			{
				renderable2D.RenderTransform = HierarchyObject.GetAttribute<Transform2D>();
			}

			RenderEngine2D.RegisterRenderable(renderable2D);

			renderable2D.RenderTransform.TransformChanged += NotifyRenderEngineChangeDetected;
		}

		private Renderable2D renderable2D;

		public void NotifyRenderEngineChangeDetected()
		{

		}
	}
}
