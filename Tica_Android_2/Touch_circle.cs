using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Tica_Android_2
{
	public class Touch_circle: Sprite
	{
		bool active;
		Texture2D selected_tex;
		Texture2D red;

		public Touch_circle ( Rectangle rect, Texture2D zelena, Texture2D crvena, float resize_scale)
		{
			rectangle =  new Rectangle((int)(rect.X*resize_scale),(int)(rect.Y*resize_scale), (int)Math.Round(rect.Width*resize_scale) , (int)Math.Round(rect.Height*resize_scale));
			texture = zelena;
			red = crvena;
			selected_tex = zelena;
		}

		public void Update(bool dobar, Rectangle touch, int x, int y)
		{
			if (!dobar) {
				if (touch.Intersects (rectangle))
					selected_tex = red;
				else {
					rectangle.X = x;
					rectangle.Y = y;
					selected_tex = texture;
					dobar = true;

				}	
			}
		}

		public void Draw(SpriteBatch sp)
		{
			sp.Draw (selected_tex, rectangle, Color.White);
		}
	}
}

