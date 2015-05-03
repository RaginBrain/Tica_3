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
	public class Tourtorial
	{
		private Sprite ruka;

		public Strelice desno_gori;
		public Strelice desno_doli;
		public Strelice livo;

		Texture2D ruka_green;
		Texture2D strel_d_g;
		Texture2D strel_d_d;
		Texture2D strel_l;
		string stanje;
		bool ruka_na_poziciji;
		private float scale;


		public void LoadContent(ContentManager cm )
		{
			ruka_green = cm.Load<Texture2D> ("Tourtorial/ruka_green");
			strel_l = cm.Load<Texture2D> ("Tourtorial/green_left");
			strel_d_d= cm.Load<Texture2D> ("Tourtorial/red_down");
			strel_d_g=cm.Load<Texture2D> ("Tourtorial/red_up");

		}

		public Tourtorial()
		{
		}
		public void Postavi_varijable(float scal)
		{

			stanje="livo";
			livo = new Strelice (scal, 200, 200,90, 200, strel_l);
			desno_doli = new Strelice (scal, 225, 225, 340, 340,strel_d_d);
			desno_gori = new Strelice (scal, 225, 200, 340, 80, strel_d_g);
			ruka=new Sprite(new Rectangle((int)livo.odrediste.X,(int)livo.odrediste.Y,(int)(scal*60),(int)(scal*60)),ruka_green);
			ruka.colision_rect = new Rectangle (0,0, (int)(scal * 30), (int)(scal * 30));
			ruka_na_poziciji = true;
		}

		public void Update(GameTime gt)
		{
			
			switch(stanje)
			{
			case ("livo"):
				ruka.colision_rect.X = ruka.rectangle.X;
				ruka.colision_rect.Y = ruka.rectangle.Y+(int)(5*scale);

				livo.Update (gt, 45,ruka.colision_rect);
				if (livo.counter >= 5) {
					livo.counter = 0;
						//ruka.rectangle.X += (1+Vector2.Distance (ruka.rectangle.Location.ToVector2 (), desno_doli.odrediste) / 10);
					ruka.rectangle.X = (int)desno_doli.odrediste.X;
					ruka.rectangle.Y = (int)desno_doli.odrediste.Y;
					stanje="desno_doli";
					}
				break;


			case ("desno_doli"):
				ruka.colision_rect.X =(int) (ruka.rectangle.X+ 50* scale);
				ruka.colision_rect.Y = (int)(ruka.rectangle.Y+50*scale);
				desno_doli.Update (gt, 115,ruka.colision_rect);
				if (desno_doli.counter >= 8) {
					desno_doli.counter = 0;
					ruka.rectangle.X = (int)(desno_gori.odrediste.X-(int)(30*scale));
					ruka.rectangle.Y = (int)desno_gori.odrediste.Y;
					stanje="desno_gori";
					}

					
				break;


			case ("desno_gori"):
				ruka.colision_rect.X = ruka.rectangle.X+(int)(45*scale);
				ruka.colision_rect.Y = ruka.rectangle.Y+(int)(30*scale);
				desno_gori.Update (gt, 105,ruka.colision_rect);
				if (desno_gori.counter >= 8) {
					desno_gori.counter = 0;
					ruka.rectangle.X = (int)livo.odrediste.X;
					ruka.rectangle.Y = (int)livo.odrediste.Y;
					stanje="livo";
				}
				break;
			}
		}

		public void Draw (SpriteBatch sp)
		{
			sp.Draw (ruka.texture, ruka.rectangle, Color.White);

			switch(stanje)
			{
			case("livo"):
				livo.Draw (sp);
				break;
			
			case("desno_doli"):
				desno_doli.Draw (sp);
				break;
			
			case("desno_gori"):
				desno_gori.Draw (sp);
				break;
			}
		}
	}
}


