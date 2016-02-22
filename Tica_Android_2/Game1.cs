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
using Android.Content;
using Android.Gms.Ads;

#endregion

namespace Tica_Android_2
{

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{	

		Oblak_Wizzard OWizz;
		Hint hint;
		public bool napravljeni;
		public int add_counter;
		public Context context;
		HealthBar akceleracija_bar;
		Sprite Score_scroll;
		Sprite support_scroll;
		Sprite Credits_Table;

		Sprite repeat_button;
		Sprite back_button;
		Jojstick joj;

		BoolButton mute_button;
		BoolButton music_button;
		BoolButton ultra_hard_mode;
		bool ultra_hard;
		bool mute_on;
		bool music_on;

		Shop game_shop;

		int racun;
		Score rezultat;
		Score ispis_brojeva;

		HS_podium hs_podium;
		Texture2D[] hs_tice;

		int uhm_udaljenostX;
		int uhm_udaljenostY;

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
		SoundEffect click;
		Song game_over_sound;

		Texture2D kruna;
		Texture2D tabla_hs;

		float maca_scale;
		Vector2 maca_origin;
		Texture2D coin_texture;

		Texture2D support;
		Texture2D[] base_skins;
		Texture2D[] stit_skins;
		Texture2D[] ranjena_skins;

		float scale;
		Texture2D txx;
		Texture2D player_textura;
		Tourtorial tourt;


		enum GameState { Start, InGame, GameOver, Turtorial, Ready, Score_show, Credits, Shop,High_score_show };
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
				if (music_on)
					sw.WriteLine ("true");
				else if (!music_on)
					sw.WriteLine ("false");
				if (ultra_hard)
					sw.WriteLine ("true");
				else if (!ultra_hard)
					sw.WriteLine ("false");
			}	
		}
		public void LelevUp()
		{
			
			scrolling1.Ubrzaj ();
			scrolling2.Ubrzaj ();
			lvlUp.brzina_kretanja = scrolling1.brzina_kretanja;
			barijera1.brzina_kretanja = scrolling1.brzina_kretanja;
			barijera2.brzina_kretanja = scrolling1.brzina_kretanja;
			barijera3.brzina_kretanja = scrolling1.brzina_kretanja;
			barijera.brzina_kretanja = scrolling1.brzina_kretanja;
			player1.speed  *= 1.1f;
			sljedeciLevel += 1;
			maca.brzina_kretanja = scrolling1.brzina_kretanja;
			maca2.brzina_kretanja = scrolling1.brzina_kretanja;
			pila.brzina_kretanja = maca.brzina_kretanja + 1*scale;

		}

		IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
		int high_score;
		int selected_bird;
		string unlocked_birds;


		List<Texture2D> lista_txtr;

		Sprite start_button;
		Sprite credits_button;
		Sprite shop_button;


		Sprite game_over;
		Stopwatch sat;
		Stopwatch camp_timer;
		int camp_position;
		bool camping;
		bool camp_trap;

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
		public InterstitialAd FinalAd;
		public adlistener intlistener;
		List<Barijera> red_prepreka;


		TouchCollection touchCollection;
		int sirina;
		int visina;

		Texture2D cntrl_arrow;
		Texture2D cntrl_jstck;

		public Game1()
		{
			
			 

			graphics = new GraphicsDeviceManager(this);

			graphics.ApplyChanges();
			graphics.IsFullScreen = true;

			sirina = graphics.PreferredBackBufferWidth;
			visina = graphics.PreferredBackBufferHeight;
			add_counter = 0;
			napravljeni = false;
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
					if (sr.ReadLine() == "true")
						music_on = true;
					else if(sr.ReadLine() == "false")
						music_on = false;
					if (sr.ReadLine() == "true")
						ultra_hard = true;
					else if(sr.ReadLine() == "false")
						ultra_hard = false;
				}

			} 
			else {
				high_score = 0;
				selected_bird = 0;
				unlocked_birds="xxx";
				racun = 0;
				mute_on = false;
				music_on = false;
				ultra_hard = false;
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
			camping = false;
			camp_trap = false;
			camp_timer = new Stopwatch ();
			rotacija_pile = 0;
			currentGameState = GameState.Start;
			upaljena_pisma_igre = false;

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
					if (sr.ReadLine() == "true")
						music_on = true;
					else if(sr.ReadLine() == "false")
						music_on = false;
					if (sr.ReadLine() == "true")
						ultra_hard = true;
					else if(sr.ReadLine() == "false")
						ultra_hard = false;
				}	
			} 
			else {
				high_score = 0;
				selected_bird = 0;
				unlocked_birds="xxx";
				racun = 0;
				mute_on = false;
				ultra_hard = false;
				music_on = false;
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
			click = Content.Load<SoundEffect>("Zvukovi/button_click");

			//*****************
			OWizz= new Oblak_Wizzard(Content,sirina,visina,scale);

			test_lista_coina = new List<Coin> ();
			txx = Content.Load<Texture2D> ("kocka");
			red_prepreka = new List<Barijera> ();

			//TODO: use this.Content to load your game content here 
			scrolling1 = new Scrolling(Content.Load<Texture2D>("bg1"), new Rectangle(0, 0, sirina, visina), 3);
			scrolling2 = new Scrolling(Content.Load<Texture2D>("bg2"), new Rectangle(sirina, 0,sirina, visina), 3);

			hint = new Hint (scale,sirina);
			hint.LoadContent (Content,scale);

			hs_tice=new Texture2D[4];
			for (int i = 0; i < 4; i++) {
				hs_tice [i] = Content.Load<Texture2D> ("HighScore/tica" + i.ToString ());
			}

			Credits_Table = new Sprite(new Rectangle((int)((sirina/2)-150*scale),(int)(0),(int)(340*scale),(int)(420*scale)),Content.Load<Texture2D> ("Botuni/credits"));

			tourt = new Tourtorial ();
			tourt.LoadContent (Content);
			tourt.Postavi_varijable (scale);

			hs_podium = new HS_podium (Content.Load<Texture2D> ("HighScore/hs_bg"), Content.Load<Texture2D> ("HighScore/fc"), hs_tice, visina, sirina,scale,selected_bird,Content.Load<Song>("HighScore/pobjeda_song"),Content.Load<Song>("HighScore/pobjeda_song_2"));
			akceleracija_bar = new HealthBar (Content.Load<Texture2D> ("okvir"), Content.Load<Texture2D> ("fill"),scale, new Rectangle ((int)(85*scale),(int)(4*scale), 120, 23));
			joj = new Jojstick (scale, visina,sirina, Content);

			base_skins= new Texture2D[4];
			stit_skins=new Texture2D[4];
			ranjena_skins=new Texture2D[4];

			for (int i = 0; i < 4; i++) 
			{
				base_skins [i] = Content.Load<Texture2D> ("Tice/tica_gotova"+i.ToString());
				stit_skins [i] = Content.Load<Texture2D> ("Tice/tica_stit"+i.ToString());
				ranjena_skins[i]=Content.Load<Texture2D> ("Tice/ranjena"+i.ToString());
			}

		
			cntrl_arrow= Content.Load<Texture2D> ("Botuni/TV_Controls_arrow");
			cntrl_jstck = Content.Load<Texture2D> ("Botuni/TV_Controls_jojstick");
		

			player1 = new Player(base_skins,stit_skins,ranjena_skins, new Rectangle((visina-(int)(visina/4.35f))/2, (visina-(int)(visina/4.35f))/2,50, 50),scale,selected_bird);
			player_textura = player1.texture;


			//dodavanje znamenaka
			lista_txtr = new List<Texture2D> ();
			for (int i = 0; i < 10; i++) {
				lista_txtr.Add (Content.Load<Texture2D> ("Znamenke/"+i.ToString()));
			}


			rezultat = new Score (lista_txtr);


			game_shop = new Shop ( selected_bird, unlocked_birds,Content, scale, sirina,lista_txtr,click);
			turtorial_tex = Content.Load<Texture2D> ("turtorial");
			kruna=Content.Load<Texture2D>("kruna");
			tabla_hs = Content.Load<Texture2D> ("high_score_table");

			Score_scroll=new Sprite(new Rectangle((int)((sirina/2)-250*scale),(int)(-280*scale),(int)(500*scale),(int)(280*scale)),Content.Load<Texture2D>("Botuni/score_scroll"));
			support_scroll=new Sprite(new Rectangle((int)((sirina/2)-190*scale),(int)(-390*scale),(int)(350*scale),(int)(380*scale)),Content.Load<Texture2D>("Botuni/pergament"));

			start_button = new Sprite (new Rectangle ((int)(sirina / 2 - 165*scale), 0, (int)(350*scale),(int)(165*scale)),Content.Load<Texture2D> ("Botuni/oblak_start"));


			credits_button = new Sprite (new Rectangle ((int)(sirina -(270*scale)),  (int)(visina - (visina / 3.7f)), (int)(150*scale),(int)(80*scale)),Content.Load<Texture2D> ("Botuni/credits button"));
			ultra_hard_mode = new BoolButton (new Rectangle ((int)(sirina -(240*scale)),  (int)(50*scale), (int)(250*scale),(int)(130*scale)),
				Content.Load<Texture2D> ("Botuni/TV_jojstick"), Content.Load<Texture2D> ("Botuni/TV_arrow"), ultra_hard);




			shop_button = new Sprite (new Rectangle ((int)(20*scale), (int)(50*scale), (int)(250*scale),(int)(130*scale)),Content.Load<Texture2D> ("Botuni/shop_oblak"));

			game_over = new Sprite (new Rectangle (-visina, 0, (int)(300*scale), (int)(409*scale)),Content.Load<Texture2D>("game_over"));
			repeat_button = new Sprite (new Rectangle ((int)(sirina - (120 * scale)),(int)(visina - (visina / 4.5f) - 80 * scale),  (int)(60 * scale), (int)(80 * scale)), Content.Load<Texture2D> ("Botuni/repeat_jaje"));
			back_button = new Sprite (new Rectangle ((int)(60 * scale),(int)(visina - (visina / 4.5f) - 80 * scale),  (int)(60 * scale), (int)(80 * scale)), Content.Load<Texture2D> ("Botuni/nazad_jaje"));
			mute_button = new BoolButton (new Rectangle ((int)(30 * scale), (int)(visina - (visina / 4.5f)), (int)(45 * scale), (int)(55 * scale)),
										Content.Load<Texture2D> ("Botuni/mute_on"), Content.Load<Texture2D> ("Botuni/mute_off"), mute_on);
			
			music_button = new BoolButton (new Rectangle ((int)(90 * scale), (int)(visina - (visina / 4.5f)), (int)(45 * scale), (int)(55 * scale)),
				Content.Load<Texture2D> ("Botuni/pisma_off"), Content.Load<Texture2D> ("Botuni/pisma_on"), music_on);
			





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
				

				game_shop.ispis_brojeva.Update (racun);
				rezultat.Update (high_score);
				touchCollection = TouchPanel.GetState ();
				Rectangle pozicija_dodira;
				foreach (TouchLocation tl in touchCollection) {

					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) {
						pozicija_dodira = new Rectangle ((int)tl.Position.X, (int)tl.Position.Y, 1, 1);

						if (tl.State == TouchLocationState.Pressed && start_button.rectangle.Intersects (pozicija_dodira)) {
							if (!mute_on)
								click.Play ();
							currentGameState = GameState.Ready;
						}
						if (tl.State == TouchLocationState.Pressed && credits_button.rectangle.Intersects (pozicija_dodira)) {
							if (!mute_on)
								click.Play ();
							currentGameState = GameState.Credits;
						}
						if (tl.State == TouchLocationState.Pressed && shop_button.rectangle.Intersects (pozicija_dodira)) {
							if (!mute_on)
								click.Play ();
							currentGameState = GameState.Shop;
						}
						if (tl.State == TouchLocationState.Pressed && mute_button.rectangle.Intersects (pozicija_dodira)) {
							mute_on = !mute_on;
						}
						if (tl.State == TouchLocationState.Pressed && ultra_hard_mode.rectangle.Intersects (pozicija_dodira)) {
							if (!mute_on)
								click.Play ();
							ultra_hard = !ultra_hard;
							ultra_hard_mode.Update (ultra_hard);
						}
						if (tl.State == TouchLocationState.Pressed && music_button.rectangle.Intersects (pozicija_dodira)) {
							music_on = !music_on;
						}
					}
				}


					
					

				if (mute_on)
					mute_button.selected_tex = mute_button.texture;
				else
					mute_button.selected_tex = mute_button.mute_off_tex;
				if (music_on)
					music_button.selected_tex = music_button.texture;
				else
					music_button.selected_tex = music_button.mute_off_tex;
				if (ultra_hard)
					ultra_hard_mode.selected_tex = ultra_hard_mode.texture;
				else
					ultra_hard_mode.selected_tex = ultra_hard_mode.mute_off_tex;
				break;

			case GameState.Credits:
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
							game_shop.Update (player1, pozicija_dodira, ref selected_bird, ref racun, ref unlocked_birds,mute_on);
							if (back_button.rectangle.Intersects (pozicija_dodira)) 
							{
								if (!mute_on)
									click.Play ();
								Save ();
								currentGameState = GameState.Start;
							}
						}
					}
				}
				break;


			case GameState.Ready:
				if (!upaljena_pisma_igre) {
					if (!music_on && !mute_on) {
						MediaPlayer.Play (pjesma_igre);
						MediaPlayer.Pause ();
						upaljena_pisma_igre = true;
						MediaPlayer.IsRepeating = true;
					}

				}
				if(!ultra_hard)
					tourt.Update (gameTime);
				touchCollection = TouchPanel.GetState ();
				foreach (TouchLocation tl in touchCollection) {
					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) {
						if (tl.State == TouchLocationState.Pressed) {
							currentGameState = GameState.InGame;
							player1.sat.Start ();
						}
					}
				}
				break;


			case GameState.InGame:

				if (Math.Abs (player1.colision_rect.Center.Y - camp_position) > 60 * scale) {
					camp_position = player1.colision_rect.Center.Y;
					camp_timer.Restart ();
					camping = false;

				} else {
					if (camp_timer.ElapsedMilliseconds > 4000 && camp_timer.ElapsedMilliseconds < 5500)
						camping = true;
					else
						camping = false;

				}


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
				hint.Update(player1,high_score,scale);

				if (add_counter>=3 && add_counter!=6) {
					FinalAd = AdWrapper.ConstructFullPageAdd (context, "ca-app-pub-9649596465496350/6705151429");
					intlistener = new adlistener ();
					intlistener.AdLoaded += () => { if (FinalAd.IsLoaded && currentGameState==GameState.Score_show)FinalAd.Show(); };
					FinalAd.AdListener = intlistener;
					add_counter = 6;
				}
			
				for (int i = 0; i < test_lista_coina.Count; i++) {
					if (test_lista_coina [i].rectangle.X < (-33 * scale))
						test_lista_coina.RemoveAt (i);
					test_lista_coina [i].Update (player1, scale, gameTime, sirina, scrolling1.brzina_kretanja,ref racun,mute_on);


				}

				rezultat.Update (player1.score);
				CoinWizz.Update ();
				stit.Update (player1, (int)(350*scale), lvlUp,scale);
				player1.Update (gameTime, visina, sirina,scale,joj,ultra_hard);

				maca2.Update (player1, ref udaljenost_barijera, visina, sirina * 3, red_prepreka,scale,stit_out,test_lista_coina,camping);
				maca.Update (player1, ref udaljenost_barijera, visina, sirina * 3, red_prepreka,scale,stit_out,test_lista_coina,camping);

				pila.Update (player1, ref udaljenost_barijera, visina, sirina * 3, red_prepreka,scale,stit_out,test_lista_coina,camping);
				rotacija_pile += 0.1f;
				if (rotacija_pile > 10)
					rotacija_pile = 0;

				//triba uštimat s velicinom sprite-a
				///////////////////////////////////////

				barijera.Update (player1,ref udaljenost_barijera, visina, sirina,red_prepreka,scale,stit_out,test_lista_coina,camping);
				barijera1.Update (player1,ref udaljenost_barijera,  visina, sirina,red_prepreka,scale,stit_out,test_lista_coina,camping);
				barijera2.Update (player1,ref udaljenost_barijera,  visina, sirina,red_prepreka,scale,stit_out,test_lista_coina,camping);
				barijera3.Update (player1,ref udaljenost_barijera,  visina, sirina,red_prepreka,scale,stit_out,test_lista_coina,camping);
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
					if (bar.rectangle.Y > (0 - bar.rectangle.Height) && (sat.ElapsedMilliseconds > 500))
						bar.rectangle.Y -= (int)(10 * scale);
				}

				player1.Update (gameTime, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth, scale,joj,ultra_hard);

				

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
						add_counter+=2;
						ultra_hard_mode.rectangle.X = (sirina);
						ultra_hard_mode.rectangle.Y = (-ultra_hard_mode.rectangle.Height);
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
							if (!mute_on)
								click.Play ();
							Initialize ();
							currentGameState = GameState.Start;
						}
					}
				}
				break;


			case GameState.Score_show:
				
				uhm_udaljenostX = (int)Math.Abs( (sirina -(448*scale) - ultra_hard_mode.rectangle.X) * scrolling1.brzina_kretanja);
				uhm_udaljenostY= (int)Math.Abs( ((265*scale) - ultra_hard_mode.rectangle.Y) * scrolling1.brzina_kretanja);



				if (ultra_hard_mode.rectangle.X > (sirina -(448*scale)-ultra_hard_mode.rectangle.Width))
					ultra_hard_mode.rectangle.X -= (int)(uhm_udaljenostX / 40);
					if (ultra_hard_mode.rectangle.Y < (265*scale)- ultra_hard_mode.rectangle.Height)
					ultra_hard_mode.rectangle.Y += (int)(uhm_udaljenostX / 40);

				if (ultra_hard)
					ultra_hard_mode.selected_tex = cntrl_jstck;
				else
					ultra_hard_mode.selected_tex = cntrl_arrow;

				if (add_counter >= 5) {
					
					add_counter = -1;
					sat.Reset ();
					sat.Start ();

					napravljeni = true;

					FinalAd.CustomBuild ();
				}
								

				if ((int)sat.Elapsed.Seconds < 4 && add_counter == -1)
					game_shop.ispis_brojeva.Update (4 - (int)(sat.Elapsed.Seconds));
		
				player1.Update (gameTime, visina, sirina, scale,joj,ultra_hard);
				rezultat.Update (player1.score);

				touchCollection = TouchPanel.GetState ();

			
				if (add_counter == -1) {
					if (support_scroll.rectangle.Y < (visina / 2 - (int)(support_scroll.rectangle.Height * 3/ 5)))
						support_scroll.rectangle.Y += (int)(10 * scale);
				}
				else {//dodat controls.change


					if (Score_scroll.rectangle.Y < (visina / 2 - (int)(Score_scroll.rectangle.Height * 5 / 6)))
						Score_scroll.rectangle.Y += (int)(15 * scale);
					OWizz.Update (scale);
				}
				foreach (TouchLocation tl in touchCollection) 
				{

					if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) 
					{
						pozicija_dodira = new Rectangle ((int)tl.Position.X, (int)tl.Position.Y, 1, 1);

						if (tl.State == TouchLocationState.Pressed && repeat_button.rectangle.Intersects (pozicija_dodira ) && (sat.Elapsed.Seconds>3 || add_counter!=-1)) {
							if (!mute_on)
								click.Play ();
							Save ();
							Initialize ();
							OWizz.Clear ();
							currentGameState = GameState.Ready;
							ultra_hard_mode.rectangle.X = (int)(sirina - (240 * scale));
							ultra_hard_mode.rectangle.Y = (int)(50 * scale);
						}
						if (tl.State == TouchLocationState.Pressed && back_button.rectangle.Intersects (pozicija_dodira)) {
							if (!mute_on)
								click.Play ();
							OWizz.Clear ();
							ultra_hard_mode.rectangle.X = (int)(sirina - (240 * scale));
							ultra_hard_mode.rectangle.Y = (int)(50 * scale);
							Save ();
							Initialize ();

						}

						if (tl.State == TouchLocationState.Pressed && ultra_hard_mode.rectangle.Intersects (pozicija_dodira)) {
							ultra_hard = !ultra_hard;
							if (!mute_on) 
								click.Play ();
							
							


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
				spriteBatch.Draw (credits_button.texture, credits_button.rectangle, Color.White);
				spriteBatch.Draw (shop_button.texture, shop_button.rectangle, Color.White);
				spriteBatch.Draw (start_button.texture, start_button.rectangle, Color.White);
				mute_button.Draw (spriteBatch);
				ultra_hard_mode.Draw (spriteBatch);
				music_button.Draw (spriteBatch);


				spriteBatch.Draw(tabla_hs,new Rectangle((int)(sirina/2.65f),(visina-visina/3)-(int)(120*scale),(int)(240*scale),(int)(230*scale)),Color.White);
				spriteBatch.Draw(kruna,new Rectangle((int)(sirina/2.1f),((visina-visina/3)-(int)(160*scale)),(int)(90*scale),(int)(60*scale)),Color.White);
				try{
					rezultat.Draw (spriteBatch, (int)(sirina/2.3f),(visina-visina/3)-(int)(60*scale),(int)(30*scale), (int)(60*scale));
				}
				catch{}
				spriteBatch.End ();

				break;

			case GameState.Credits:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);

				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);

				spriteBatch.Draw (Credits_Table.texture, Credits_Table.rectangle, Color.White);
				spriteBatch.Draw (back_button.texture, back_button.rectangle, Color.White);
				spriteBatch.Draw(Score_scroll.texture, Score_scroll.rectangle,Color.White);
				spriteBatch.End ();

				break;

			case GameState.Shop:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);

				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);

				game_shop.Draw (spriteBatch,scale);

				spriteBatch.Draw (back_button.texture, back_button.rectangle, Color.White);
				spriteBatch.End ();

				break;
			
			case GameState.Ready:
				spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);
				scrolling1.Draw (spriteBatch);
				scrolling2.Draw (spriteBatch);

				try {
					rezultat.Draw (spriteBatch, 5, 5, (int)(15 * scale), (int)(40 * scale));
				} catch {
				}

				if (!ultra_hard) {
					spriteBatch.Draw (turtorial_tex, new Rectangle (player1.colision_rect.X + player1.colision_rect.Width - (visina - (int)(visina / 4.35f)) / 2, player1.colision_rect.Y + player1.colision_rect.Height - (visina - (int)(visina / 4.35f)) / 2, (visina - (int)(visina / 4.35f)), (visina - (int)(visina / 4.35f))), Color.White);
					tourt.Draw (spriteBatch);
				}
				else{
					spriteBatch.Draw (
						player_textura
						, new Vector2(player1.rectangle.X+(scale*player1.playerAnimation.FrameWith/2),player1.rectangle.Y+(scale*player1.playerAnimation.FrameHeight/2))
						, player1.playerAnimation.suorceRect, Color.White, 0, player1.playerAnimation.origin,scale*0.96f, SpriteEffects.None, 0
					);
					joj.Draw (spriteBatch);
				}

				spriteBatch.End ();

				break;


			case GameState.InGame:
				{

					GraphicsDevice.Clear (Color.CornflowerBlue);

					// TODO: Add your drawing code here
					spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.NonPremultiplied);
					scrolling1.Draw (spriteBatch);
					scrolling2.Draw (spriteBatch);
					if(ultra_hard && joj.postavljen)
						joj.Draw (spriteBatch);
					hint.Draw (spriteBatch);
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

				OWizz.Draw (spriteBatch);
				player1.draw_ranjena (spriteBatch);

				spriteBatch.Draw (repeat_button.texture, repeat_button.rectangle, Color.White);
				spriteBatch.Draw (back_button.texture, back_button.rectangle, Color.White);
				spriteBatch.Draw (Score_scroll.texture, Score_scroll.rectangle, Color.White);
				spriteBatch.Draw (support_scroll.texture, support_scroll.rectangle, Color.White);


				if (add_counter == -1) {
					if (sat.Elapsed.Seconds < 4)
						game_shop.ispis_brojeva.Draw (spriteBatch, (int)(sirina - (90 * scale)), (int)(visina - (visina / 4.5f) - 85 * scale), (int)(25 * scale), (int)(40 * scale));
					rezultat.Draw (spriteBatch,Score_scroll.rectangle.Center.X-(int)((player1.score.ToString().Length)*20*scale) ,support_scroll.rectangle.Y+(int)(250*scale),(int)(25*scale), (int)(40*scale));
				}
				else
					ultra_hard_mode.Draw (spriteBatch);
				try{
					rezultat.Draw (spriteBatch, (int)(support_scroll.rectangle.Center.X/1.25f-(player1.score.ToString().Length)*20*scale),Score_scroll.rectangle.Center.Y-(int)(20*scale),(int)(30*scale), (int)(50*scale));
				}
				catch{}


				spriteBatch.End ();

				break;
			}

			base.Draw(gameTime);
		}
	}
}