﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;


namespace Tica_Android_2
{
	public class Sprite
	{


		public float brzina_kretanja;
		public float speed_buffer;

		public Texture2D texture;
		public Rectangle rectangle;
		public Rectangle colision_rect;
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, rectangle, Color.White);
		}

		public Sprite(Rectangle rect,Texture2D tex)
		{
			rectangle = rect;
			texture = tex;
		}

		public Sprite()
		{
		}
	}

	public class Scrolling : Sprite
	{
		public float speed_step;
		public void Ubrzaj()
		{
			brzina_kretanja += speed_step;
		}
		public Scrolling(Texture2D newTexture, Rectangle newRectangle,int brzina)
		{
			texture = newTexture;
			rectangle = newRectangle;
			brzina_kretanja = brzina;
			speed_step = brzina_kretanja * .25f;
			speed_buffer = 0;
		}

		public void Update(float speed_scale)
		{
			speed_buffer += brzina_kretanja*speed_scale;
			if (speed_buffer > 1) {
				rectangle.X -=(int)speed_buffer;
				speed_buffer-=(float)((int)speed_buffer);
			}
		}

	}

	public  class Player : Sprite
	{
		public Animation playerAnimation=new Animation();
		public float speed_step;
		bool maknut;
		bool game_over_charge;
		public bool ubodena_s_liva;

		public Stopwatch sat;

		Texture2D[] niz_base_skin;
		Texture2D[] niz_stit_skin;
		Texture2D[] niz_ranjena_skin;

		public TouchCollection touchCollection;
		public Texture2D texture_stit;
		public Texture2D ranjena;
		public float speed;

		public Vector2 Velocity;

		public int score;
		public bool alive;
		public bool stit;


		private int udajenost_X;
		private int udaljenost_Y;
		private Point centar;
		private float pomakX;
		private float pomakY;

		public float Speed
		{
			get { return speed; }
			set { speed = value; }
		}

		public void Initialize()
		{
		}

		public void Ubrzaj()
		{
			speed += speed_step;
		}
		public Player(Texture2D[] tex,Texture2D[] tex_stit,Texture2D[] tex_ranjena,Rectangle rect,float resize_scale, int selct)
		{
			sat = new Stopwatch ();
			niz_base_skin= tex;
			niz_stit_skin=tex_stit;
			niz_ranjena_skin= tex_ranjena;

			maknut = false; // true kad stiscemo joystick
			speed = 6*resize_scale;
			speed_step = speed * .15f;
			texture = niz_base_skin[selct];
			texture_stit = niz_stit_skin[selct];
			ranjena = niz_ranjena_skin [selct];
			rectangle = new Rectangle(rect.X, rect.Y, (int)Math.Round(rect.Width*resize_scale) , (int)Math.Round(rect.Height*resize_scale));
			alive = true;
			score = 0;
			stit = false;
			playerAnimation.brzina_animacije = 7;
			playerAnimation.broj_framova_sirina = 5;
			playerAnimation.broj_framova_visina = 3;
			playerAnimation.Image = tex[selct];
			playerAnimation.origin = new Vector2 (tex[selct].Width / 10, tex[selct].Height / 6);
			colision_rect = new Rectangle (rect.X, rect.Y, (int)(rectangle.Width*0.8f), (int)(rectangle.Height * 0.6f));
			game_over_charge = false;
			ubodena_s_liva = false;
		



		}

		public void ProminiSkin(int selct)
		{
			texture =niz_base_skin[selct];
			texture_stit = niz_stit_skin[selct];
			ranjena = niz_ranjena_skin[selct];
		}

		public void Update(GameTime gameTime,int visina_ekrana, int duljina,float resize_scale, Jojstick jstc,bool ultra)
		{
			if (alive) {
				maknut = false;
				playerAnimation.active = true;
				float faktor_X;
				float faktor_Y;


				if ((float)udajenost_X / (150f * resize_scale) < 1)
					faktor_X = (0.6f + (udajenost_X / (150f * resize_scale)));
				else
					faktor_X = 1.6f;

				if ((float)udaljenost_Y / (150f * resize_scale) < 1)
					faktor_Y = (0.6f + udaljenost_Y / (150f * resize_scale));
				else
					faktor_Y = 1.6f;

				//kretanje sa ubrzanim gibanjem *******************************************
				touchCollection=TouchPanel.GetState();

				foreach (TouchLocation tl in touchCollection) 
				{

				
					if ((tl.State == TouchLocationState.Pressed)
						|| (tl.State == TouchLocationState.Moved))
					{
						//******************************
						if (!ultra) {
							maknut = true;
							centar = colision_rect.Center;
							udaljenost_Y = (int)(Math.Abs (centar.Y - tl.Position.Y));
							udajenost_X = (int)(Math.Abs (centar.X - tl.Position.X));


							if (tl.Position.Y < colision_rect.Y + colision_rect.Height / 2) {
								if (Velocity.Y > 0)
									Velocity.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds * faktor_Y;
								Velocity.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
							} else if (tl.Position.Y > colision_rect.Y + colision_rect.Height / 2) {
								if (Velocity.Y < 0)
									Velocity.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds * faktor_Y;
								Velocity.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds * faktor_Y * 2;
							}

							if (tl.Position.X < colision_rect.X + colision_rect.Width / 2) {
								if (Velocity.X > 0)
									Velocity.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds * faktor_X;
								Velocity.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds * faktor_X * 2;
							} else if (tl.Position.X > colision_rect.X + colision_rect.Width / 2) {
								if (Velocity.X < 0)
									Velocity.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds * faktor_X;
								Velocity.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds * faktor_X * 2;
							}
						} 
						else {
							maknut = true;
							jstc.update (tl, speed, resize_scale);

//							if(jstc.FaktX*Velocity.X<0)
//								Velocity.X += speed * jstc.FaktX * jstc.postotak*(float)gameTime.ElapsedGameTime.TotalSeconds*3;
//							if(jstc.FaktY*Velocity.Y<0)
//								Velocity.Y += speed * jstc.FaktY* jstc.postotak *(float)gameTime.ElapsedGameTime.TotalSeconds*3;	


							Velocity.X += speed *jstc.bonus* jstc.FaktX * jstc.postotak*(float)gameTime.ElapsedGameTime.TotalSeconds*1.75f;
							Velocity.Y += speed *jstc.bonus* jstc.FaktY* jstc.postotak *(float)gameTime.ElapsedGameTime.TotalSeconds*1.75f;

							pomakX =resize_scale*jstc.FaktX/ Math.Abs(jstc.FaktX)+ (.50f*speed * jstc.bonus * jstc.FaktX * jstc.postotak);
							pomakY =resize_scale*jstc.FaktY/ Math.Abs(jstc.FaktY)+ (.50f*speed * jstc.bonus * jstc.FaktY * jstc.postotak);

							if ((colision_rect.Y > (visina_ekrana - visina_ekrana / 4.35f)) && pomakY > 0)
								pomakY = 0;
							if ((colision_rect.Y < 0) && pomakY < 0)
								pomakY = 0;
							
							if ((colision_rect.X < -5) && pomakX < 0)
								pomakX = 0;
							if (colision_rect.X > duljina - colision_rect.Width && pomakX > 0)
								pomakX = 0;
								

							

							rectangle.Y += (int)(Math.Round (pomakY));
							rectangle.X += (int)(Math.Round (pomakX));
						}
						//****************************
					}
				}
				if (!maknut) {

					if (ultra) {
						jstc.Vracaj_Tocku ();
					}
					sat.Restart ();

					if (Velocity.X > 0)
						Velocity.X -= Velocity.X / 30f;
					if (Velocity.X < 0)
						Velocity.X += Math.Abs (Velocity.X) / 30f;

					if (Velocity.Y > 0)
						Velocity.Y -= Velocity.Y / 30f;
					if (Velocity.Y < 0)
						Velocity.Y += Math.Abs (Velocity.Y) / 30f;
				}



				rectangle.X += (int)(Math.Round (Velocity.X));
				rectangle.Y += (int)(Math.Round (Velocity.Y));

				if (!stit) {
					colision_rect.X = rectangle.X + (int)(resize_scale * playerAnimation.FrameWith / 3.5f);
					colision_rect.Y = rectangle.Y + (int)(resize_scale * playerAnimation.FrameHeight / 2.75f);
				} else {
					colision_rect.X = rectangle.X + (int)(resize_scale * playerAnimation.FrameWith / 3.75f);
					colision_rect.Y = rectangle.Y + (int)(resize_scale * playerAnimation.FrameHeight / 3f);
				}

				//kretanje zavrsava ovdje ***************************************************

		

				//uvjet za odbijanje, svi faktori su empiristički uštimani*******************
				if ((colision_rect.Y > (visina_ekrana - visina_ekrana / 4.35f)) || (colision_rect.Y < 0)) {
					rectangle.Y -= (int)(Velocity.Y * 3f);
					Velocity.Y = -Velocity.Y * 0.65f;
				}
				if ((colision_rect.X < -5 && Velocity.X < 0) || colision_rect.X > duljina - colision_rect.Width && Velocity.X > 0)
					Velocity.X = 0;

				playerAnimation.Position = new Vector2 (rectangle.Location.X, rectangle.Location.Y);
				playerAnimation.Update (gameTime);
			}

			//kad je ranjena
			else
			{
				if (game_over_charge == false) {
					Velocity.Y = -5*resize_scale;
					if (ubodena_s_liva)
						Velocity.X = -3 * resize_scale;
					else
						Velocity.X = 3 * resize_scale;
					game_over_charge = true;
				}
				if(Velocity.X>0)
					Velocity.X-= speed * (float)gameTime.ElapsedGameTime.TotalSeconds*.2f;
				else if(Velocity.X<0)
					Velocity.X+= speed * (float)gameTime.ElapsedGameTime.TotalSeconds*.2f;

				Velocity.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds*1.5f;
				colision_rect.Y += (int)Velocity.Y;


				rectangle.Y += (int)Velocity.Y;
				

				rectangle.X += (int)Velocity.X;
			}
		}

		public float Totalna_akceleracija()
		{
			return (Math.Abs (Velocity.X) + Math.Abs (Velocity.Y));
		}
		public void draw_ranjena(SpriteBatch sb)
		{
			sb.Draw (ranjena, new Rectangle((rectangle.X+(int)(rectangle.Width*0.22f)),rectangle.Y+(int)(rectangle.Height*0.2f),(int)(rectangle.Width*1.45f),(int)(rectangle.Height*1.4f)), Color.White);
		}

	}


	public class Znamenka:Sprite
	{
		public bool vidljiv;
		public int broj;
		public int pozicija;
		public List<Texture2D> lista_textura;
		public Znamenka()
		{
			lista_textura = new List<Texture2D> ();
		}

	}

	public class Score
	{

		public List<Znamenka> lista_znamenki;

		public void Update(int rezultat)
		{
			string s = rezultat.ToString();
			for (int i = 0; i < 7; i++)
			{
				if (i < s.Length) 
				{
					lista_znamenki [i].vidljiv = true;
					int ind = int.Parse (s [i].ToString ());
					lista_znamenki [i].pozicija = i;
					lista_znamenki [i].broj = ind;
					lista_znamenki [i].texture = lista_znamenki [i].lista_textura [ind];
				} 
				else
					lista_znamenki [i].vidljiv = false;
			}
		}

		public Score(List<Texture2D> teksture)
		{
			lista_znamenki = new List<Znamenka> ();
			for (int i = 0; i < 7; i++)
			{
				Znamenka x = new Znamenka ();
				x.lista_textura = teksture;
				lista_znamenki.Add (x);
			} 
		}


		public void Draw(SpriteBatch sb,int x,int y, int sirina,int visina)
		{
			int poz_X = x;
			foreach(Znamenka z in lista_znamenki)
			{
				if (z.vidljiv) 
				{
					sb.Draw (z.texture, new Rectangle (poz_X, y, sirina, visina), Color.White);
					poz_X += sirina;
				}
			}
		}


	}
}
