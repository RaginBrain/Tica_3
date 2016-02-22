using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Tica_Android_2
{
	public class Shop
	{
		Skin_button[] Bird_select_button;
		int selected_skin;
		Texture2D Lock;
		Texture2D Check_sign;
		Sprite scroll;
		public Score ispis_brojeva;
		Sprite vise_dijamanata;
		Texture2D upitnik;
		SoundEffect reject;
		SoundEffect Click;



		public Shop (int selected,string unlocked_b,ContentManager cm,float scale,int sirina, List<Texture2D> znamenke,SoundEffect click)
		{
			ispis_brojeva = new Score (znamenke);
			scroll = new Sprite (new Rectangle((int)(sirina/2 - 255*scale),(int)(100*scale),(int)(550*scale),(int)(300*scale)),cm.Load<Texture2D>("Shop/scroll"));
			Lock =cm.Load<Texture2D> ("Shop/lock");
			Check_sign = cm.Load<Texture2D> ("Shop/check");
			upitnik = cm.Load<Texture2D> ("Shop/upitnik");
			reject = cm.Load<SoundEffect>("Zvukovi/error_button");

			Bird_select_button = new Skin_button[4];

			vise_dijamanata = new Sprite(new Rectangle((int)(300*scale),(int)(320*scale),(int)(70*scale),(int)(50*scale)),cm.Load<Texture2D>("Shop/broj_dj"));
			Bird_select_button [0] = new Skin_button (new Rectangle ((int)(sirina/2 - 205*scale), (int)(150 * scale), (int)(100 * scale), (int)(100 * scale)),cm.Load<Texture2D> ("Shop/tica_0"),0);
			Bird_select_button [1] = new Skin_button (new Rectangle ((int)(sirina/2 - 100*scale), (int)(150 * scale), (int)(100 * scale), (int)(100 * scale)),cm.Load<Texture2D> ("Shop/tica_1"),10);
			Bird_select_button [2] = new Skin_button (new Rectangle ((int)(sirina/2 +5*scale), (int)(150 * scale), (int)(100 * scale), (int)(100 * scale)),cm.Load<Texture2D> ("Shop/tica_2"),15 );
			Bird_select_button [3] = new Skin_button (new Rectangle ((int)(sirina/2 +105*scale), (int)(150* scale), (int)(100 * scale), (int)(100 * scale)), cm.Load<Texture2D> ("Shop/tica_3"),30);
			selected_skin = selected;
			for (int i = 0; i < 3; i++)
			{
				if (unlocked_b [i] == 'x')
					Bird_select_button [i + 1].locked = true;
				else if (unlocked_b [i] == 'o')
					Bird_select_button [i + 1].locked = false;
					
			}
			Click = click;
		}

		public void Locked_Birds(ref string locked_birds)
		{
			locked_birds = "";
			for(int i=1; i<4; i++)
			{
				if(Bird_select_button[i].locked)
					locked_birds+="x";
				else
					locked_birds+="o";
			}
		}

		public void Update(Player player1, Rectangle pozicija_dodira, ref int selected_bird, ref int racun, ref string l_b,bool mute_on)
		{
			
			for (int i = 0; i < 4; i++) 
				{
					if (Bird_select_button [i].rectangle.Intersects (pozicija_dodira)) 
					{
					
					if (Bird_select_button [i].locked && racun >= Bird_select_button [i].price && Bird_select_button [i-1].locked==false) {
						if (!mute_on)
							Click.Play ();
						racun -= Bird_select_button [i].price;
						Bird_select_button [i].locked = false;
						Locked_Birds (ref l_b);
						ispis_brojeva.Update (racun);
					} 
					else if(Bird_select_button [i].locked)
					{
						if (!mute_on)
							reject.Play ();
					}

						if (!Bird_select_button [i].locked) 
						{
							player1.ProminiSkin (i);
							selected_bird = i;
							selected_skin = i;
						}
					}
				}
		}

		public void Draw(SpriteBatch spriteBatch,float scale)
		{
			scroll.Draw (spriteBatch);

			for (int i = 0; i < 4; i++) {

				if(i==0 || !Bird_select_button [i-1].locked)
					spriteBatch.Draw (Bird_select_button[i].texture, Bird_select_button[i].rectangle, Color.White);
				else
					spriteBatch.Draw (upitnik, Bird_select_button [i].rectangle, Color.White);

				if (Bird_select_button [i].locked) {
					if (!Bird_select_button [i-1].locked)
						spriteBatch.Draw (Lock, Bird_select_button [i].rectangle, Color.White);
				}
				if (i==selected_skin)
					spriteBatch.Draw (Check_sign, Bird_select_button [i].rectangle, Color.White);
				
			}
			vise_dijamanata.Draw (spriteBatch);
			ispis_brojeva.Draw (spriteBatch, (int)(360*scale),(int)(280*scale),(int)(20*scale), (int)(50*scale));
		}
	}
}

