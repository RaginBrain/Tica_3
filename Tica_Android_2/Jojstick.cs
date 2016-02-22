using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;


namespace Tica_Android_2
{
	public class Jojstick
	{
		Sprite pozadina;
		Sprite tocka;
		Vector2 centar;
		float scal;
		public float bonus;
		public float postotak;
		public float udaljenost;
		public float radius;
		float udaljenostX;
		float udaljenostY;
		public bool dobar;
		public float FaktX;
		public float FaktY;
		public bool postavljen;


		public Jojstick (float scale, int visina,int sirina , ContentManager cm)
		{
			scal = scale;
			radius = 100 * scale;
			pozadina = new Sprite
				(new Rectangle(sirina-(int)(200*scale),visina-(int)(200*scale),(int)(200*scale),(int)(200*scale))
					,cm.Load<Texture2D>("jojstic_pozadina"));
			centar = pozadina.rectangle.Center.ToVector2 ();
			tocka = new Sprite
				(new Rectangle((int)(20*scale),-(int)(190*scale),(int)(48*scale),(int)(48*scale))
					,cm.Load<Texture2D> ("Botuni/jojstic_button"));
			postavljen = false;

		}

		public void update(TouchLocation tl, float speed,float scale)
		{
			if (!postavljen) {
				pozadina.rectangle.X = (int)(tl.Position.X - (100 * scale));
				pozadina.rectangle.Y = (int)(tl.Position.Y - (100 * scale));
				centar = pozadina.rectangle.Center.ToVector2 ();
			}

			postavljen = true;
			udaljenost = Vector2.Distance (tl.Position, centar);

			if (udaljenost < radius) {
				dobar = true;
				postotak = .3f + udaljenost / radius;	


				udaljenostX = Math.Abs (tl.Position.X - centar.X);
				udaljenostY = Math.Abs (tl.Position.Y - centar.Y);

				FaktX = (udaljenostX * udaljenostX) / (udaljenost * udaljenost);
				FaktY = (udaljenostY * udaljenostY) / (udaljenost * udaljenost);


				if (tl.Position.X < centar.X)
					FaktX = -FaktX;
				if (tl.Position.Y < centar.Y)
					FaktY = -FaktY;
				if (Math.Abs(FaktX) < Math.Abs(FaktY))
					bonus = 1f+ (1.45f * FaktX * FaktX);
				else if (Math.Abs(FaktY) < Math.Abs(FaktX))
					bonus =1f + (1.45f * FaktY * FaktY);

				tocka.rectangle.X = (int)(tl.Position.X - (24 * scale));
				tocka.rectangle.Y = (int)(tl.Position.Y - (24 * scale));
			} 

			else if(udaljenost<3f*radius)
			{
				float size_postotak = radius / udaljenost;
				dobar = true;
				postotak = 1;
				udaljenostX = Math.Abs (tl.Position.X - centar.X);
				udaljenostY = Math.Abs (tl.Position.Y - centar.Y);

				FaktX = (udaljenostX * udaljenostX) / (udaljenost * udaljenost);
				FaktY = (udaljenostY * udaljenostY) / (udaljenost * udaljenost);

				udaljenostX = Math.Abs (tl.Position.X - centar.X)*size_postotak;
				udaljenostY = Math.Abs (tl.Position.Y - centar.Y)*size_postotak;



				if (tl.Position.X < centar.X)
					FaktX = -FaktX;
				if (tl.Position.Y < centar.Y)
					FaktY = -FaktY;


				if (Math.Abs(FaktX) < Math.Abs(FaktY))
					bonus = 1f+ (1.55f * FaktX * FaktX);
				else if (Math.Abs(FaktY) < Math.Abs(FaktX))
					bonus =1f + (1.55f * FaktY * FaktY);
				
				if(FaktX>0)
					tocka.rectangle.X = (int)(centar.X + udaljenostX- tocka.rectangle.Width/2);
				else
					tocka.rectangle.X = (int)(centar.X - udaljenostX - tocka.rectangle.Width/2);

				if(FaktY>0)
					tocka.rectangle.Y = (int)(centar.Y+ udaljenostY- tocka.rectangle.Height/2);
				else
					tocka.rectangle.Y = (int)(centar.Y- udaljenostY- tocka.rectangle.Height/2);

			}
			else {
				bonus = 1;
				postotak = 0;
				dobar = false;
				FaktX = 0;
				FaktY = 0;
				udaljenost = 0;
				tocka.rectangle.X = pozadina.rectangle.Center.X - (int)(24 * scale);
				tocka.rectangle.Y = pozadina.rectangle.Center.Y - (int)(24 * scale);


			}
		}

		public void Draw(SpriteBatch sp)
		{
			pozadina.Draw (sp);
			tocka.Draw (sp);
		}

		public void Vracaj_Tocku()
		{
			postavljen = false;
			if (tocka.rectangle.Center.X  > pozadina.rectangle.Center.X)
				tocka.rectangle.X -= (int)Math.Abs (tocka.rectangle.Center.X - pozadina.rectangle.Center.X)/10;
			if (tocka.rectangle.Center.X  < pozadina.rectangle.Center.X)
				tocka.rectangle.X += (int)Math.Abs (tocka.rectangle.Center.X - pozadina.rectangle.Center.X)/10;
			if (tocka.rectangle.Center.Y  > pozadina.rectangle.Center.Y)
				tocka.rectangle.Y -= (int)Math.Abs (tocka.rectangle.Center.Y - pozadina.rectangle.Center.Y)/10;
			if (tocka.rectangle.Center.Y  < pozadina.rectangle.Center.Y)
				tocka.rectangle.Y += (int)Math.Abs (tocka.rectangle.Center.Y - pozadina.rectangle.Center.Y)/10;


		}
	}
}

