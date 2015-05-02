using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
namespace Tica_Android_2
{
	public class Strelice
	{
		public Vector2 odrediste;
		public Vector2 ishodiste;
		float scal;

		float speed_buffer;
		Sprite[] niz_spritova; 

		public int counter;

		public Strelice (float scale, int ish_x,int ish_y, int odr_x,int odr_y, Texture2D tex)
		{
			scal = scale;
			int udaljenost_x = (int)((odr_x*scale)-(ish_x*scale))/4;
			int udaljenost_y = (int)((odr_y*scale)-(ish_y*scale) )/4;
			speed_buffer = 0;
			niz_spritova = new Sprite[3];

			ishodiste = new Vector2 (scale * ish_x, scale * ish_y);
			odrediste = new Vector2 (scale * odr_x, scale * odr_y);

			for (int i = 0; i < 3; i++) {
				niz_spritova [i] = new Sprite ();
				niz_spritova [i].rectangle = new Rectangle ((int)(ish_x*scale + (i * udaljenost_x)), (int)(ish_y*scale+ (i* udaljenost_y)), (int)(30 * scale), (int)(30 * scale));
				niz_spritova [i].texture = tex;
			}
		}

		public void Update(GameTime gmt,int speed,Rectangle coll)
		{
			speed_buffer +=  (float)(speed*scal * gmt.ElapsedGameTime.TotalSeconds);


				if (speed_buffer > 1)
				{
					if (niz_spritova [0].texture.Name == "Tourtorial/green_left") {
					for (int i = 0; i < 3; i++) {
						niz_spritova [i].rectangle.X -= (int)speed_buffer;

					}
					speed_buffer = 0;
					} 
					else if (niz_spritova [0].texture.Name == "Tourtorial/red_down") {
					for (int i = 0; i < 3; i++) {
						niz_spritova [i].rectangle.X += (int)speed_buffer;
						niz_spritova [i].rectangle.Y += (int)speed_buffer;

					}
					speed_buffer = 0;
					}
					else if (niz_spritova [0].texture.Name == "Tourtorial/red_up") {
					for (int i = 0; i < 3; i++) {
						niz_spritova [i].rectangle.X += (int)speed_buffer;
						niz_spritova [i].rectangle.Y -= (int)speed_buffer;

					}
					speed_buffer = 0;
					}
				}
			for (int i = 0; i < 3; i++) {
				if (niz_spritova [i].rectangle.Intersects (coll)) {
					niz_spritova [i].rectangle.X = (int)ishodiste.X;
					niz_spritova [i].rectangle.Y = (int)ishodiste.Y;
					counter++;
				}
			}

		}
		public void Draw(SpriteBatch sp)
		{
			for (int i = 0; i < 3; i++) {
				sp.Draw (niz_spritova [i].texture, niz_spritova [i].rectangle, Color.White);
			}
		}
	}
}

