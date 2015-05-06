using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Tica_Android_2
{
	public class HS_podium :Sprite
	{

		private bool song_started;
		Song pobjeda_song, pobjeda_song2;
		Texture2D pozadina;
		Texture2D front_cloud;
		Texture2D[] podium_bird;
		int visina;
		int sirina;
		Random r = new Random();

		public HS_podium (Texture2D p, Texture2D fc, Texture2D[] pb, int v, int s,float scale,int sel,Song pisma, Song pisma2)
		{
			song_started = false;
			pobjeda_song = pisma;
			pobjeda_song2 = pisma2;

			speed_buffer = 0;
			podium_bird = pb;
			texture = podium_bird[sel];
			pozadina = p;
			front_cloud = fc;
			visina = v;
			sirina = s;
			rectangle = new Rectangle ((int)(sirina/2-(160*scale)) , visina, (int)(310*scale), (int)(350*scale));
		}


		public void Update(int selected_bird, GameTime gameTime, bool mute_ON)
		{	
			
			if (!song_started && (!mute_ON)) {
				MediaPlayer.IsRepeating = false;
				if (r.Next (0, 3) != 1)
					MediaPlayer.Play (pobjeda_song);
				else
					MediaPlayer.Play (pobjeda_song2);
				song_started=true;
			}

			texture = podium_bird [selected_bird];
			if (rectangle.Y > (visina - rectangle.Height))
			{
				speed_buffer += (visina/13f* (float)gameTime.ElapsedGameTime.TotalSeconds);
				if (speed_buffer > 1) {
					speed_buffer--;
					rectangle.Y--;
				}
			}
			
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (pozadina, new Rectangle (0, 0, sirina, visina), Color.White);
			spriteBatch.Draw (texture, rectangle, Color.White);
			spriteBatch.Draw (front_cloud, new Rectangle (0, 0, sirina, visina+1), Color.White);
		}
	}
}

