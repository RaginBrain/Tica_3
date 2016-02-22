using System;
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
	public class BoolButton:Sprite
	{
		public Texture2D mute_off_tex;
		public Texture2D selected_tex;

		public BoolButton(Rectangle r, Texture2D mON, Texture2D mOFF, bool mute_on)
		{
			
			rectangle = r;
			texture = mON;
			mute_off_tex = mOFF;
			if (mute_on)
				selected_tex = texture;
			else
				selected_tex = mute_off_tex;
		}

		public void Draw(SpriteBatch sp)
		{
			sp.Draw (selected_tex, rectangle, Color.White);
		}

		public void Update(bool mute_on)
		{
			if (mute_on)
				selected_tex = texture;
			else
				selected_tex = mute_off_tex;
		}
	
	}
}

