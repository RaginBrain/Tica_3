#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

#endregion

namespace Tica_Android_2
{

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{	

		HealthBar akceleracija_bar;
		Sprite Score_scroll;
		Sprite repeat_button;
		Sprite back_button;

		MuteBotun mute_button;
		bool mute_on;

		Shop game_shop;
		Texture2D lokot;
		Texture2D check;
		int racun;
		Skin_button[] Bird_select_button;
		Score rezultat;
		Score ispis_brojeva;

		HS_podium hs_podium;
		Texture2D[] hs_tice;

		bool upaljena_pisma_igre;
		Song pjesma_igre;

		CoinWizzard CoinWizz;
		List<Coin> test_lista_coina;
		//Coin test_coin;
		Texture2D turtorial_tex;
		Texture2D dijamant_tex;

		Song stit_out;
		SoundEffect coin_zvuk;
		SoundEffect dijamant_zvuk;
		Song game_over_sound;

		Texture2D kruna;
		float maca_scale;
		Vector2 maca_origin;
		Texture2D coin_texture;

		Texture2D[] base_skins;
		Texture2D[] stit_skins;
		Texture2D[] ranjena_skins;

		float scale;
		Texture2D txx;
		Texture2D player_textura;
		Tourtorial tourt;


		enum GameState { Start, InGame, GameOver, Turtorial, Ready, Score_show, Options, Shop,High_score_show };
		GameState currentGameState = GameState.InGame;


		public void Save()
		{
			IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream ("high_score.txt", FileMode.OpenOrCreate, FileAccess.Write);
			using (StreamWriter sw = new StreamWriter (isoStream)) {
				sw.Flush ();
				sw.WriteLine (high_score.ToString ());
				sw.WriteLine (selected_bird.ToString ());
				sw.WriteLine (unlocked_birds);
				sw.WriteLine (racun.ToString ());
				if (mute_on)
					sw.WriteLine ("true");
				else if (!mute_on)
					sw.WriteLine ("false");
			}	
		}
		public void LelevUp()
		{
			scrolling1.brzina_kretanja += 0.6f*scale;
			scrolling2.brzina_kretanja += 0.6f*scale;
			lvlUp.brzina_kretanja = scrolling1.brzina_kretanja;
			barijera1.brzina_kretanja += 0.6f*scale;
			barijera2.brzina_kretanja += 0.6f*scale;
			barijera3.brzina_kretanja += 0.6f*scale;
			barijera.brzina_kretanja += 0.6f*scale;
			player1.speed += (0.6f*scale);
			sljedeciLevel += 1;
			maca.brzina_kretanja += 0.6f*scale;
			maca2.brzina_kretanja += 0.6f*scale;
			pila.brzina_kretanja = maca.brzina_kretanja + 1*scale;

		}

		IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
		int high_score;
		int selected_bird;
		string unlocked_birds;


		List<Texture2D> lista_txtr;

		Sprite start_button;
		Sprite options_button;
		Sprite shop_button;
		Sprite centar_hint;
		bool centar_hint_bool;

		Sprite game_over;
		Stopwatch sat;

		Barijera maca,maca2;
		Barijera pila;
		float rotacija_pile;
		Vector2 pila_origin;
		float pila_scale;

		Stit stit;
		LevelUp lvlUp;
		public GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Barijera barijera,barijera2;
		PokretnaBarijera barijera1, barijera3;
		Random r = new Random();

		int udaljenost_barijera;

		int sljedeciLevel;
		Scrolling scrolling1;
		Scrolling scrolling2;
		Player player1;

		List<Barijera> red_prepreka;


		TouchCollection touchCollection;
		int sirina;
		int visina;

		public Game1()
		{

			graphics = new GraphicsDeviceManager(this);

			graphics.ApplyChanges();
			graphics.IsFullScreen = true;

			sirina = graphics.PreferredBackBufferWidth;
			visina = graphics.PreferredBackBufferHeight;

			scale = ((float)(((float)visina / 480f) + ((float)sirina / 800f)) / 2f);
			Content.RootDirectory = "Content";

			if (savegameStorage.FileExists ("high_score.txt")) {
				IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream ("high_score.txt", FileMode.OpenOrCreate, FileAccess.Read);
				using (StreamReader sr = new StreamReader (isoStream)) {
					high_score = int.Parse (sr.ReadLine ());
					selected_bird=int.Parse (sr.ReadLine ());
					unlocked_birds = sr.ReadLine ();
					racun = int.Parse (sr.ReadLine ());
					if (sr.ReadLine() == "true")
						mute_on = true;
					else if(sr.ReadLine() == "false")
						mute_on = false;

				}	
			} 
			else {
				high_score = 0;
				selected_bird = 0;
				unlocked_birds="xxx";
				racun = 0;
				mute_on = false;
				Save ();
			}
		}




		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
			player1.Initialize();
			player1.playerAnimation.Initialize();
			sljedeciLevel = lvlUp.level + 1;
			sat = new Stopwatch ();
			rotacija_pile = 0;
			currentGameState = GameState.Start;
			upaljena_pisma_igre = false;
			centar_hint_bool = false;

			//gamesave
			if (savegameStorage.FileExists ("high_score.txt")) {
				IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream ("high_score.txt", FileMode.OpenOrCreate, FileAccess.Read);
				using (StreamReader sr = new StreamReader (isoStream)) {
					high_score = int.Parse (sr.ReadLine ());
					selected_bird=int.Parse (sr.ReadLine ());
					unlocked_birds = sr.ReadLine ();
					racun = int.Parse (sr.ReadLine ());
					if (sr.ReadLine () == "true")
						mute_on = true;
					else
						mute_on = false;
				}	
			} 
			else {
				high_score = 0;
				selected_bird = 0;
				unlocked_birds="xxx";
				racun = 0;
				mute_on = false;
				Save ();	
			}
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//ZVUKOVI
			stit_out = Content.Load<Song> ("Zvukovi/BubblePop");
			coin_zvuk = Content.Load<SoundEffect> ("Zvukovi/coin_zvuk");
			dijamant_zvuk= Content.Load<SoundEffect>("Zvukovi/dijamant_zvuk");
			game_over_sound = Content.Load<Song>("Zvukovi/game_over_sound");
			pjesma_igre = Content.Load<Song> ("Zvukovi/pisma_igre");

			//*****************
			test_lista_coina = new List<Coin> ();
			txx = Content.Load<Texture2D> ("kocka");
			red_prepreka = new List<Barijera> ();

			//TODO: use this.Content to load your game content here 
			scrolling1 = new Scrolling(Content.Load<Texture2D>("bg1"), new Rectangle(0, 0, sirina, visina), 3);
			scrolling2 = new Scrolling(Content.Load<Texture2D>("bg2"), new Rectangle(sirina, 0,sirina, visina), 3);

			hs_tice=new Texture2D[4];
			for (int i = 0; i < 4; i++) {
				hs_tice [i] = Content.Load<Texture2D> ("HighScore/tica" + i.ToString ());
			}

			tourt = new Tourtorial ();
			tourt.LoadContent (Content);
			tourt.Postavi_varijable (scale);

			hs_podium = new HS_podium (Content.Load<Texture2D> ("HighScore/hs_bg"), Content.Load<Texture2D> ("HighScore/fc"), hs_tice, visina, sirina,scale,selected_bird,Content.Load<Song>("HighScore/pobjeda_song"),Content.Load<Song>("HighScore/pobjeda_song_2"));
			akceleracija_bar = new HealthBar (Content.Load<Texture2D> ("okvir"), Content.Load<Texture2D> ("fill"),scale, new Rectangle ((int)(85*scale),(int)(4*scale), 120, 23));

			base_skins= new Texture2D[4];
			stit_skins=new Texture2D[4];
			ranjena_skins=new Texture2D[4];

			for (int i = 0; i < 4; i++) 
			{
				base_skins [i] = Content.Load<Texture2D> ("Tice/tica_gotova"+i.ToString());
				stit_skins [i] = Content.Load<Texture2D> ("Tice/tica_stit"+i.ToString());
				ranjena_skins[i]=Content.Load<Texture2D> ("Tice/ranjena"+i.ToString());
			}

		

			Bird_select_button = new Skin_button[4];
			Bird_select_button [0] = new Skin_button (new Rectangle ((int)(sirina/2 - 205*scale), (int)(150 * scale), (int)(100 * scale), (int)(100 * scale)),Content.Load<Texture2D> ("Shop/tica_0"),0);
			Bird_select_button [1] = new Skin_button (new Rectangle ((int)(sirina/2 - 100*scale), (int)(150 * scale), (int)(100 * scale), (int)(100 * scale)),Content.Load<Texture2D> ("Shop/tica_1"),0);
			Bird_select_button [2] = new Skin_button (new Rectangle ((int)(sirina/2 +5*scale), (int)(150 * scale), (int)(100 * scale), (int)(100 * scale)),Content.Load<Texture2D> ("Shop/tica_2"),0 );
			Bird_select_button [3] = new Skin_button (new Rectangle ((int)(sirina/2 +105*scale), (int)(150* scale), (int)(100 * scale), (int)(100 * scale)), Content.Load<Texture2D> ("Shop/tica_3"),0);

			lokot =Content.Load<Texture2D> ("Shop/lock");
			check = Content.Load<Texture2D> ("Shop/check");

			game_shop = new Shop (Bird_select_button, lokot, check, selected_bird, unlocked_birds);

			player1 = new Player(base_skins,stit_skins,ranjena_skins, new Rectangle((visina-(int)(visina/4.35f))/2, (visina-(int)(visina/4.35f))/2,50, 50),scale,selected_bird);
			player_textura = player1.texture;


			//dodavanje znamenaka
			lista_txtr = new List<Texture2D> ();
			for (int i = 0; i < 10; i++) {
				lista_txtr.Add (Content.Load<Texture2D> ("Znamenke/"+i.ToString()));
			}


			rezultat = new Score (lista_txtr);
			ispis_brojeva = new Score (lista_txtr);

			turtorial_tex = Content.Load<Texture2D> ("turtorial");
			kruna=Content.Load<Texture2D>("kruna");

			Score_scroll=new Sprite(new Rectangle((int)((sirina/2)-190*scale),(int)(-260*scale),(int)(380*scale),(int)(260*scale)),Content.Load<Texture2D>("Botuni/score_scroll"));

			start_button = new Sprite (new Rectangle ((int)(sirina / 2 - 165*scale), 0, (int)(350*scale),(int)(165*scale)),Content.Load<Texture2D> ("Botuni/oblak_start"));
			options_button = new Sprite (new Rectangle ((int)(sirina -(270*scale)),  (int)(50*scale), (int)(250*scale),(int)(130*scale)),Content.Load<Texture2D> ("Botuni/options_oblak"));
			shop_button = new Sprite (new Rectangle ((int)(20*scale), (int)(50*scale), (int)(250*scale),(int)(130*scale)),Content.Load<Texture2D> ("Botuni/shop_oblak"));
			centar_hint = new Sprite (new Rectangle ((int)(sirina +100*scale),  (int)(50*scale), (int)(220*scale),(int)(125*scale)),Content.Load<Texture2D> ("Botuni/centar_hint"));

			game_over = new Sprite (new Rectangle (-visina, 0, (int)(300*scale), (int)(409*scale)),Content.Load<Texture2D>("game_over"));
			repeat_button = new Sprite (new Rectangle ((int)(sirina - (120 * scale)),(int)(visina - (visina / 4.5f) - 80 * scale),  (int)(60 * scale), (int)(80 * scale)), Content.Load<Texture2D> ("Botuni/repeat_jaje"));
			back_button = new Sprite (new Rectangle ((int)(60 * scale),(int)(visina - (visina / 4.5f) - 80 * scale),  (int)(60 * scale), (int)(80 * scale)), Content.Load<Texture2D> ("Botuni/nazad_jaje"));
			mute_button = new MuteBotun (new Rectangle ((int)(200 * scale), (int)(visina - (visina / 6f)), (int)(50 * scale), (int)(50 * scale)),
			Content.Load<Texture2D> ("Botuni/mute_on"), Content.Load<Texture2D> ("Botuni/mute_off"), mute_on);
			






			udaljenost_barijera = (int)(300*scale);
			coin_texture = Content.Load<Texture2D> ("coin");
			dijamant_tex = Content.Load<Texture2D> ("dijamant");
			CoinWizz = new CoinWizzard (coin_zvuk,dijamant_zvuk,coin_texture,dijamant_tex);
			stit = new Stit(Content.Load<Texture2D>("stit"), new Rectangle(2000, 50, 64, 64),scale);
			lvlUp = new LevelUp(Content.Load<Texture2D>("be_ready"), new Rectangle(1500, (int)(visina-(48*scale)), 110, 37),scale);

			//*****************************
			barijera = new Barijera(Content.Load<Texture2D>("barijera"), new Rectangle(udaljenost_barijera*3, 0, 35, 100),scale,CoinWizz);
			red_prepreka.Add (barijera);
			maca2 = new Barijera(Content.Load<Texture2D>("maca"), new Rectangle(red_prepreka[0].rectangle.X+udaljenost_barijera, 250,  80,  80),scale,CoinWizz);
			red_prepreka.Add (maca2);
			barijera1 = new PokretnaBarijera(Content.Load<Texture2D>("barijera"),
				new Rectangle(red_prepreka[1].rectangle.X+udaljenost_barijera, 100, 35, 100), false, visina,scale,CoinWizz);
			red_prepreka.Add (barijera1);
			//pila----------------
			pila = new Barijera(Content.Load<Texture2D>("pila"), new Rectangle(red_prepreka[2].rectangle.X+udaljenost_barijera, 300,  80,  80),scale,CoinWizz);
			red_prepreka.Add (pila);
			//--------------------
			barijera2 = new Barijera(Content.Load<Texture2D>("barijera"), new Rectangle(red_prepreka[3].rectangle.X+udaljenost_barijera, 250, 35, 100),scale,CoinWizz);
			red_prepreka.Add (barijera2);
			barijera3 = new PokretnaBarijera(Content.Load<Texture2D>("barijera"), new Rectangle(red_prepreka[4].rectangle.X+udaljenost_barijera, 50, 35, 100), true, sirina,scale,CoinWizz);
			red_prepreka.Add (barijera3);
			maca = new Barijera(Content.Load<Texture2D>("maca"), new Rectangle(red_prepreka[5].rectangle.X+udaljenost_barijera,200,  80,  80),scale,CoinWizz);
			red_prepreka.Add (maca);
			pila_origin = new Vector2 (pila.texture.Width/ 2, pila.texture.Height/ 2);
			pila_scale = ((float)(pila.rectangle.Width) / (float)(pila.texture.Width))*1.3f;
			maca_scale= ((float)(maca.rectangle.Width) / (float)(maca.texture.Width))*1.3f;
			maca_origin = new Vector2 (maca.texture.Width/ 2, maca.texture.Height/ 2);

			//*****************************
			foreach (Barijera bar in red_prepreka) {
				CoinWizz.Ubaci_Coine(test_lista_coina, visina, scale,bar);
			}


		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			switch (currentGameState)
			{	

			case GameState.Start:
				ispis_brojeva.Update(racun);
				rezultat.Update (high_score);
				touchCollection = TouchPanel.GetState ();
				Rectangle pozicija_dodira;
				foreach (TouchLocation tl in touchCollection) 
				{

					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) 
					{
						pozicija_dodira = new Rectangle ((int)tl.Position.X, (int)tl.Position.Y, 1, 1);

						if (tl.State == TouchLocationState.Pressed && start_button.rectangle.Intersects (pozicija_dodira))
							currentGameState = GameState.Ready;
						if (tl.State == TouchLocationState.Pressed && options_button.rectangle.Intersects (pozicija_dodira))
							currentGameState = GameState.Options;
						if (tl.State == TouchLocationState.Pressed && shop_button.rectangle.Intersects (pozicija_dodira))
							currentGameState = GameState.Shop;
						if (tl.State == TouchLocationState.Pressed && mute_button.rectangle.Intersects (pozicija_dodira)) {
							mute_on = !mute_on;
						}
					}
				}
				if (mute_on)
					mute_button.selected_tex = mute_button.texture;
				else
					mute_button.selected_tex = mute_button.mute_off_tex;
				break;

			case GameState.Options:
				touchCollection = TouchPanel.GetState ();
				foreach (TouchLocation tl in touchCollection) {
					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) {

						pozicija_dodira = new Rectangle ((int)tl.Position.X, (int)tl.Position.Y, 1, 1);
						if (tl.State == TouchLocationState.Pressed && back_button.rectangle.Intersects (pozicija_dodira))
							currentGameState = GameState.Start;
					}
				}
				break;

			case GameState.Shop:
				touchCollection = TouchPanel.GetState ();
				foreach (TouchLocation tl in touchCollection) {
					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) {
						pozicija_dodira = new Rectangle ((int)tl.Position.X, (int)tl.Position.Y, 1, 1);
						if (tl.State == TouchLocationState.Pressed)
						{
							game_shop.Update (player1, pozicija_dodira, ref selected_bird, ref racun, ref unlocked_birds);
							if (back_button.rectangle.Intersects (pozicija_dodira)) 
							{
								Save ();
								currentGameState = GameState.Start;
							}
						}
					}
				}
				break;


			case GameState.Ready:
				if (!upaljena_pisma_igre) {
					if (!mute_on)
						MediaPlayer.Play (pjesma_igre);
					MediaPlayer.Pause ();
					upaljena_pisma_igre = true;
					MediaPlayer.IsRepeating = true;
				}
				tourt.Update (gameTime);
				touchCollection = TouchPanel.GetState ();
				foreach (TouchLocation tl in touchCollection) {
					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) {
						if (tl.State == TouchLocationState.Pressed)
							currentGameState = GameState.InGame;
					}
				}
				break;


			case GameState.InGame:

				// Pozadina*********************************************************************
				if (upaljena_pisma_igre == true) {
					MediaPlayer.Resume ();
					upaljena_pisma_igre = false;
				}

				akceleracija_bar.Update (player1.Totalna_akceleracija(), 20f*scale);
				if (scrolling1.rectangle.X + scrolling1.rectangle.Width <= 0)
					scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling1.rectangle.Width;
				if (scrolling2.rectangle.X + graphics.PreferredBackBufferWidth <= 0)
					scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.rectangle.Width;
				scrolling1.Update (scale);
				scrolling2.Update (scale);
				//******************************************************************************


			
				for (int i = 0; i < test_lista_coina.Count; i++) {
					if (test_lista_coina [i].rectangle.X < (-33 * scale))
						test_lista_coina.RemoveAt (i);
					test_lista_coina [i].Update (player1, scale, gameTime, sirina, scrolling1.brzina_kretanja,ref racun,mute_on);


				}

				rezultat.Update (player1.score);
				CoinWizz.Update ();
				stit.Update (player1, (int)(350*scale), lvlUp,scale);
				player1.Update (gameTime, visina, sirina,scale);

				maca2.Update (player1, ref udaljenost_barijera, visina, sirina * 3, red_prepreka,scale,stit_out,test_lista_coina);
				maca.Update (player1, ref udaljenost_barijera, visina, sirina * 3, red_prepreka,scale,stit_out,test_lista_coina);

				pila.Update (player1, ref udaljenost_barijera, visina, sirina * 3, red_prepreka,scale,stit_out,test_lista_coina);
				rotacija_pile += 0.1f;
				if (rotacija_pile > 10)
					rotacija_pile = 0;

				//triba uštimat s velicinom sprite-a
				///////////////////////////////////////

				barijera.Update (player1,ref udaljenost_barijera, visina, sirina,red_prepreka,scale,stit_out,test_lista_coina);
				barijera1.Update (player1,ref udaljenost_barijera,  visina, sirina,red_prepreka,scale,stit_out,test_lista_coina);
				barijera2.Update (player1,ref udaljenost_barijera,  visina, sirina,red_prepreka,scale,stit_out,test_lista_coina);
				barijera3.Update (player1,ref udaljenost_barijera,  visina, sirina,red_prepreka,scale,stit_out,test_lista_coina);
				lvlUp.Update (player1, (int)(300*scale), ref udaljenost_barijera, scale);

				if (sljedeciLevel == lvlUp.level)
					LelevUp ();

				touchCollection = TouchPanel.GetState ();


				if (player1.stit)
					player_textura=player1.texture_stit;
				else
					player_textura = player1.texture;
				if (player1.alive == false) {
					MediaPlayer.Stop ();
					currentGameState = GameState.GameOver;
					if (player1.score < high_score) {
						MediaPlayer.IsRepeating = false;
						if(!mute_on)
							MediaPlayer.Play (game_over_sound);
					}
					
						

				}
				break;

			case GameState.GameOver:
				//cekaj
				if (sat.ElapsedMilliseconds == 0 || sat.ElapsedMilliseconds > 2400)
					sat.Restart ();

				foreach (Barijera bar in red_prepreka) {
					if (bar.rectangle.Y > (0 - bar.rectangle.Height)&&(sat.ElapsedMilliseconds > 500))
						bar.rectangle.Y-=(int)(10*scale);
				}
				if (player1.score < 650 && player1.rectangle.X > (sirina - 300 * scale) && centar_hint_bool == false)
					centar_hint_bool = true;

				player1.Update (gameTime, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth,scale);

				

				if (sat.ElapsedMilliseconds > 1200) 
				{
					if (player1.score > high_score) {
						IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream ("high_score.txt", FileMode.OpenOrCreate, FileAccess.Write);
						using (StreamWriter sw = new StreamWriter (isoStream)) {
							sw.Flush ();
							sw.WriteLine (player1.score.ToString ());
							sw.WriteLine (selected_bird.ToString ());
							sw.WriteLine (unlocked_birds);
							sw.WriteLine (racun.ToString ());
							if (mute_on)
								sw.WriteLine ("true");
							else if (!mute_on)
								sw.WriteLine ("false");
						}	
						currentGameState = GameState.High_score_show;
					}
					 else {
						Save ();
						currentGameState = GameState.Score_show;
					}
				}
				break;

			//podiumrise
			case GameState.High_score_show:

				//pisma_start
				hs_podium.Update (selected_bird, gameTime,mute_on);
				touchCollection = TouchPanel.GetState ();
				foreach (TouchLocation tl in touchCollection) {
					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) {

						pozicija_dodira = new Rectangle ((int)tl.Position.X, (int)tl.Position.Y, 1, 1);
						if (tl.State == TouchLocationState.Pressed && back_button.rectangle.Intersects (pozicija_dodira))
						{
							MediaPlayer.Stop ();
							Initialize ();
							currentGameState = GameState.Start;
						}
					}
				}
				break;


			case GameState.Score_show:

				player1.Update (gameTime, visina, sirina, scale);
				rezultat.Update (player1.score);
				touchCollection = TouchPanel.GetState ();

				if (centar_hint_bool && centar_hint.rectangle.X > (sirina - 200 * scale))
					centar_hint.rectangle.X -= (int)(10 * scale);

				if (Score_scroll.rectangle.Y < (visina / 2 - (int)(Score_scroll.rectangle.Height*3/4)))
					Score_scroll.rectangle.Y += (int)(15 * scale);
				foreach (TouchLocation tl in touchCollection) 
				{

					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) 
					{
						pozicija_dodira = new Rectangle ((int)tl.Position.X, (int)tl.Position.Y, 1, 1);

						if (tl.State == TouchLocationState.Pressed && repeat_button.rectangle.Intersects (pozicija_dodira)) {
							Initialize ();
							currentGameState = GameState.Ready;
						}
						if (tl.State == TouchLocationState.Pressed && back_button.rectangle.Intersects (pozicija_dodira)) {
							Initialize ();

						}

					}


				}
				break;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			switch (currentGameState) 
			{
			case GameState.Start:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);

				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);
				spriteBatch.Draw (options_button.texture, options_button.rectangle, Color.White);
				spriteBatch.Draw (shop_button.texture, shop_button.rectangle, Color.White);
				spriteBatch.Draw (start_button.texture, start_button.rectangle, Color.White);
				mute_button.Draw (spriteBatch);

				spriteBatch.Draw(kruna,new Rectangle((sirina/2),((visina-visina/3)-(int)(65*scale)),(int)(90*scale),(int)(60*scale)),Color.White);
				try{
					rezultat.Draw (spriteBatch, sirina/2,visina-visina/3,(int)(30*scale), (int)(80*scale));
					ispis_brojeva.Draw (spriteBatch, 0,visina-visina/3,(int)(20*scale), (int)(50*scale));

				}
				catch{}
				spriteBatch.End ();

				break;

			case GameState.Options:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);

				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);

				spriteBatch.Draw (back_button.texture, back_button.rectangle, Color.White);
				spriteBatch.Draw(Score_scroll.texture, Score_scroll.rectangle,Color.White);
				spriteBatch.End ();

				break;

			case GameState.Shop:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);

				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);

				game_shop.Draw (spriteBatch);

				spriteBatch.Draw (back_button.texture, back_button.rectangle, Color.White);
				spriteBatch.End ();

				break;
			
			case GameState.Ready:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);
				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);

				spriteBatch.Draw(turtorial_tex,new Rectangle(player1.colision_rect.X+player1.colision_rect.Width-(visina-(int)(visina/4.35f))/2,player1.colision_rect.Y+player1.colision_rect.Height-(visina-(int)(visina/4.35f))/2,(visina-(int)(visina/4.35f)),(visina-(int)(visina/4.35f))),Color.White);
				try{
					rezultat.Draw (spriteBatch, 5, 5, (int)(15*scale), (int)(40*scale));
				}
				catch{}
				tourt.Draw (spriteBatch);

				spriteBatch.End ();

				break;


			case GameState.InGame:
				{

					GraphicsDevice.Clear (Color.CornflowerBlue);

					// TODO: Add your drawing code here
					spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);
					scrolling1.Draw (spriteBatch);
					scrolling2.Draw (spriteBatch);

					foreach (Coin x in test_lista_coina) 
					{
						x.Draw (spriteBatch, scale);
					}

					lvlUp.Draw (spriteBatch);

					try{
						rezultat.Draw (spriteBatch, 5, 5, (int)(15*scale), (int)(40*scale));
					}
					catch{}

					stit.Draw (spriteBatch);
					barijera.Draw (spriteBatch);
					barijera1.Draw (spriteBatch);
					barijera2.Draw (spriteBatch);
					barijera3.Draw (spriteBatch);

					spriteBatch.Draw (maca.texture, new Vector2(maca.rectangle.X+(int)(maca.rectangle.Width/2),maca.rectangle.Y+(int)(maca.rectangle.Height/2)), null, Color.White, 0, maca_origin,maca_scale, SpriteEffects.None, 0);
					spriteBatch.Draw (maca2.texture, new Vector2(maca2.rectangle.X+(int)(maca2.rectangle.Width/2),maca2.rectangle.Y+(int)(maca2.rectangle.Height/2)), null, Color.White, 0, maca_origin,maca_scale, SpriteEffects.None, 0);
					spriteBatch.Draw (pila.texture, new Vector2(pila.rectangle.X+(int)(pila.rectangle.Width/2),pila.rectangle.Y+(int)(pila.rectangle.Height/2)), null, Color.White, rotacija_pile, pila_origin, (pila_scale), SpriteEffects.None, 0);


					spriteBatch.Draw (
						player_textura
						, new Vector2(player1.rectangle.X+(scale*player1.playerAnimation.FrameWith/2),player1.rectangle.Y+(scale*player1.playerAnimation.FrameHeight/2))
						, player1.playerAnimation.suorceRect, Color.White, 0, player1.playerAnimation.origin,scale*0.96f, SpriteEffects.None, 0
					);
					akceleracija_bar.Draw (spriteBatch);
					spriteBatch.End ();
					break;
				}
			case GameState.GameOver:
				GraphicsDevice.Clear (Color.CornflowerBlue);

				// TODO: Add your drawing code here
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);

				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);
				//spriteBatch.Draw (game_over.texture, game_over.rectangle, Color.White);
				player1.draw_ranjena (spriteBatch);
				
				barijera.Draw (spriteBatch);
				barijera1.Draw (spriteBatch);
				barijera2.Draw (spriteBatch);
				barijera3.Draw (spriteBatch);
				spriteBatch.Draw (maca.texture, new Vector2(maca.rectangle.X+(int)(maca.rectangle.Width/2),maca.rectangle.Y+(int)(maca.rectangle.Height/2)), null, Color.White, 0, maca_origin,maca_scale, SpriteEffects.None, 0);
				spriteBatch.Draw (maca2.texture, new Vector2(maca2.rectangle.X+(int)(maca2.rectangle.Width/2),maca2.rectangle.Y+(int)(maca2.rectangle.Height/2)), null, Color.White, 0, maca_origin,maca_scale, SpriteEffects.None, 0);
				spriteBatch.Draw (pila.texture, new Vector2(pila.rectangle.X+(int)(pila.rectangle.Width/2),pila.rectangle.Y+(int)(pila.rectangle.Height/2)), null, Color.White, rotacija_pile, pila_origin, (pila_scale), SpriteEffects.None, 0);

				spriteBatch.End ();

				break;
			case GameState.High_score_show:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);
				hs_podium.Draw (spriteBatch);
				spriteBatch.Draw (back_button.texture, back_button.rectangle, Color.White);
				spriteBatch.End ();

				break;


			case GameState.Score_show:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);

				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);

				player1.draw_ranjena (spriteBatch);

				spriteBatch.Draw (repeat_button.texture, repeat_button.rectangle, Color.White);
				spriteBatch.Draw (back_button.texture, back_button.rectangle, Color.White);
				spriteBatch.Draw(Score_scroll.texture, Score_scroll.rectangle,Color.White);
				spriteBatch.Draw (centar_hint.texture, centar_hint.rectangle, Color.White);

				try{
					rezultat.Draw (spriteBatch, Score_scroll.rectangle.Center.X-(int)((player1.score.ToString().Length)*20*scale),Score_scroll.rectangle.Center.Y,(int)(30*scale), (int)(50*scale));
				}
				catch{}


				spriteBatch.End ();

				break;
			}

			base.Draw(gameTime);
		}
	}
}