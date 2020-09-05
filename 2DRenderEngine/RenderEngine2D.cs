using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CrystalClear.RenderEngine2D
{
	// TODO: make instance and then allow setting one instance to one window?
	// TODO: deal gracefully with running out of slots for renderables.
	public static class RenderEngine2D
	{
		private static Dictionary<uint, Renderable2D> renderables = new Dictionary<uint, Renderable2D>();

		private static Queue<uint> availableIds = new Queue<uint>();

		internal static void RegisterRenderable(Renderable2D renderable2D)
		{
			renderable2D.RenderableId = GetUniqueId();
		}

		internal static void UnRegisterRenderable(uint renderableId)
		{
			renderables.Remove(renderableId);

			availableIds.Enqueue(renderableId);
		}

		private static uint GetUniqueId()
		{
			if (availableIds.Count != 0)
			{
				return availableIds.Dequeue();
			}
			else
			{
				return (uint)renderables.Count + 1;
			}
		}
	}
}
