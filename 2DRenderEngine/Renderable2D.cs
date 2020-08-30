using CrystalClear.ScriptUtilities;
using SFML.Graphics;

namespace CrystalClear.RenderEngine2D
{
	public class Renderable2D
	{
		public Sprite Sprite;

		public Transform2D RenderTransform;

		public uint RenderableId { get; internal set; }
	}
}