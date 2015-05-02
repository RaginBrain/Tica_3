using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Tica_Android_2
{
	public class HealthBar:Sprite
	{
		Texture2D fill;
		int full;
		int current_value;
		float scale;
		float postotak;
		Color col;

		public HealthBar (Texture2D okvir, Texture2D bar,float resize_scale,Rectangle rect)
		{
			rectangle =  new Rectangle(rect.X,rect.Y, (int)Math.Round(rect.Width*resize_scale) , (int)Math.Round(rect.Height*resize_scale));
			full = rectangle.Width;
			texture = okvir;
			fill = bar;
			scale = resize_scale;
		}

		public void Update(float akceleracija, float maksimalna)
		{
			
			if (akceleracija < maksimalna) {
				if(akceleracija/maksimalna<.10f)
					current_value=0;
				else
					current_value = (int)((akceleracija / maksimalna) * rectangle.Width);
				postotak = akceleracija / maksimalna;
			} 
			else if(akceleracija > maksimalna){
				current_value = rectangle.Width;
				postotak = 1f;
			}


			if (postotak < .70f)
				col = Color.Green;
			else if (postotak < .80f)
				col = Color.OrangeRed;
			else
				col = Color.Red;
		}

		public void Draw(SpriteBatch sp)
		{
			
			sp.Draw (fill, new Rectangle (rectangle.X, rectangle.Y, current_value, rectangle.Height), col);
			sp.Draw (texture, rectangle, Color.White);
		}
	}
}

