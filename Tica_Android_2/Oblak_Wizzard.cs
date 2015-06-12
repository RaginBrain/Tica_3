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
	public class Oblak_Wizzard
	{
		Texture2D how;
		Texture2D to;
		Texture2D fly;
		List<Sprite> Lista_oblaka;
		Texture2D tekstura;
		Stopwatch sat;
		Random r;
		int sir;
		int vis;

		public Oblak_Wizzard (ContentManager cm, int sirina, int visina,float scale)
		{
			r = new Random ();
			tekstura = cm.Load<Texture2D> ("Botuni/oblak_upitnik");
			how = cm.Load<Texture2D> ("Botuni/how");
			to = cm.Load<Texture2D> ("Botuni/to");
			fly = cm.Load<Texture2D> ("Botuni/fly");

			Lista_oblaka = new List<Sprite> ();
			sat = new Stopwatch ();
			sir = sirina;
			vis = visina - (int)(300*scale);
		}

		public void Update(float scale)
		{
			sat.Start ();
			if (Lista_oblaka.Count < 50) 
			{
				if (sat.ElapsedMilliseconds > 200) 
				{
					sat.Reset ();
					if (Lista_oblaka.Count == 0) {
						Lista_oblaka.Add (new Sprite (
							new Rectangle ((int)(200 * scale), (int)(320 * scale)
								, (int)(150 * scale), (int)(80 * scale)), how));
					}

					else if (Lista_oblaka.Count == 1) {
						Lista_oblaka.Add (new Sprite (
							new Rectangle ((int)(350 * scale), (int)(320 * scale)
								, (int)(120 * scale), (int)(80 * scale)), to));
					}
					else if (Lista_oblaka.Count == 2)
					{
						Lista_oblaka.Add (new Sprite (
							new Rectangle ((int)(470 * scale), (int)(320 * scale)
								, (int)(150 * scale), (int)(80 * scale)), fly));
					}
					else 
					{
						
						Sprite s=new Sprite (
							new Rectangle (r.Next (0, sir), r.Next (0, vis)
								, (int)(r.Next (65, 100) * scale), (int)(r.Next (45, 65) * scale)), tekstura);
						bool dobar = true;
						foreach (Sprite spr in Lista_oblaka)
						{
							if (spr.rectangle.Intersects (s.rectangle))
								dobar = false;
						}
						if(dobar)
						Lista_oblaka.Add (s);
					}
				} 
			}
			else
				sat.Stop ();
			
		}

		public void Clear()
		{
			Lista_oblaka.Clear ();
		}


		public void Draw(SpriteBatch sb)
		{
			foreach (Sprite s in Lista_oblaka)
				s.Draw (sb);

		}

	}
}

