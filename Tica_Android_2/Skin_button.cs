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
	public class Skin_button :Sprite
	{
		public bool locked;
		public int price;

		public Skin_button (Rectangle rect,Texture2D tex,int p)
		{
			rectangle = rect;
			texture = tex;
			locked = false;
			price = p;
		}


	}
}

