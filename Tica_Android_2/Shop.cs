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
	public class Shop
	{
		Skin_button[] Bird_select_button;
		int selected_skin;
		Texture2D Lock;
		Texture2D Check_sign;
		Rectangle pozicija_dodira;


		public Shop (Skin_button[] B, Texture2D l,Texture2D c,int selected,string unlocked_b)
		{
			Check_sign = c;
			Lock = l;
			Bird_select_button = B;
			selected_skin = selected;
			for (int i = 0; i < 3; i++)
			{
				if (unlocked_b [i] == 'x')
					Bird_select_button [i + 1].locked = true;
				else if (unlocked_b [i] == 'o')
					Bird_select_button [i + 1].locked = false;
					
			}

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

		public void Update(Player player1, Rectangle pozicija_dodira, ref int selected_bird, ref int racun, ref string l_b)
		{
			
			for (int i = 0; i < 4; i++) 
				{
					if (Bird_select_button [i].rectangle.Intersects (pozicija_dodira)) 
					{
					
						if (Bird_select_button [i].locked && racun >= Bird_select_button [i].price)
						{
							racun -= Bird_select_button [i].price;
							Bird_select_button [i].locked = false;
							Locked_Birds (ref l_b);
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

		public void Draw(SpriteBatch spriteBatch)
		{

			for (int i = 0; i < 4; i++) {
				
				spriteBatch.Draw (Bird_select_button[i].texture, Bird_select_button[i].rectangle, Color.White);
				if (Bird_select_button [i].locked)
					spriteBatch.Draw (Lock, Bird_select_button [i].rectangle, Color.White);
				if (i==selected_skin)
					spriteBatch.Draw (Check_sign, Bird_select_button [i].rectangle, Color.White);
				
			}
		}
	}
}

