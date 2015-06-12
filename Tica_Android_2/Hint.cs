using System;
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
using Android.Gms.Games;

namespace Tica_Android_2
{
	public class Hint
	{
		Texture2D strelica;
		Sprite kvadrat;
		Stopwatch sat;
		int start_point;
		bool upaljen;
		bool prikazi;
		int strel_X;
		Rectangle r1;
		Rectangle r2;
		Rectangle r3;
		public void LoadContent(ContentManager cm, float scale)
		{
			strelica = cm.Load<Texture2D> ("Botuni/h_strelica");
			kvadrat.texture = cm.Load<Texture2D> ("Botuni/kvadrat");

			kvadrat.rectangle = new Rectangle ((int)(50 * scale), (int)(20 * scale), (int)(450 * scale), (int)(350 * scale));
			upaljen = false;
			prikazi = false;

		}

		public Hint (float scale,int sirina)
		{
			kvadrat = new Sprite ();
			sat = new Stopwatch ();
			r1 = new Rectangle ((int)(sirina-(110*scale)), (int)(10 * scale), (int)(100 * scale), (int)(50 * scale));
			r2 = new Rectangle ((int)(sirina-(110*scale)), (int)(180 * scale), (int)(100 * scale), (int)(50 * scale));
			r3 = new Rectangle ((int)(sirina-(110*scale)), (int)(320 * scale), (int)(100 * scale), (int)(50 * scale));

		}

		public void Update(Player p1,int h_score, float scale)
		{
			if (p1.colision_rect.Center.X > (500 * scale)) {
				if (upaljen) {
					if (sat.ElapsedMilliseconds >= 1600 && h_score < 1200)
						prikazi = true;	
				} else {
					sat.Restart ();
					upaljen = true;
				}
			} 
			else {
				upaljen = false;
				prikazi = false;
			}

		}

		public void Draw(SpriteBatch sb)
		{
			if (prikazi && (sat.ElapsedMilliseconds % 1000) > 500) 
			{
				sb.Draw (strelica, r1, Color.White);
				sb.Draw (strelica, r2, Color.White);
				sb.Draw (strelica, r3, Color.White);
				sb.Draw (kvadrat.texture, kvadrat.rectangle, Color.White);
			}
		}
	}
}

