using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sleepwalker.Screens;
using DDW.Display;
using System.Reflection;
using System;
using Sleepwalker.audio;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework;

namespace Sleepwalker
{
	public class SleepwalkerGame : V2DGame
    {
        Cue bq;
        Cue dq;
		public SleepwalkerGame() : base()
        {
            Components.Add(new GamerServicesComponent(this));
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            FontManager.Instance.AddFont("Impact", V2DGame.contentManager.Load<SpriteFont>(@"Impact"));
            FontManager.Instance.AddFont("Inkpen2 Chords", V2DGame.contentManager.Load<SpriteFont>(@"Inkpen2"));
            stage.InitializeAudio(@"Content\audio\sleepwalker.xgs", @"Content\audio\Wave Bank.xwb", @"Content\audio\Sound Bank.xsb");
            stage.InitializeMusic(@"Content\audio\sleepwalker2.xgs", @"Content\audio\MusicWaveBank.xwb", @"Content\audio\MusicSoundBank.xsb");

            //Guide.SimulateTrialMode = true;
        }

		//public override bool HasCursor { get { return true; } }
        protected override void CreateScreens()
        {
            stage.AddScreen(new TitleScreen(new SymbolImport("menuScreens", "titleScreens"))); 

            stage.AddScreen(new Tutorial0Screen(new SymbolImport("gameScreens", "tutorial0Screen"))); // how to move
            stage.AddScreen(new Tutorial1Screen(new SymbolImport("gameScreens", "tutorial1Screen")));  
            stage.AddScreen(new Tutorial2Screen(new SymbolImport("gameScreens", "tutorial2Screen")));  
            stage.AddScreen(new Tutorial3Screen(new SymbolImport("gameScreens", "tutorial3Screen")));  
            stage.AddScreen(new Tutorial4Screen(new SymbolImport("gameScreens", "tutorial4Screen")));  
            stage.AddScreen(new Tutorial5Screen(new SymbolImport("gameScreens", "tutorial5Screen")));

            stage.AddScreen(new Level1Screen(new SymbolImport("gameScreens", "level1Screen")));     //1 elevator
            stage.AddScreen(new Level2Screen(new SymbolImport("gameScreens", "level2Screen")));     //2 beam elevators
            stage.AddScreen(new Level7Screen(new SymbolImport("gameScreens", "level7Screen")));     //3 big leap
            stage.AddScreen(new Level11Screen(new SymbolImport("gameScreens", "level11ScreenA"), 0));   //2 piano A
            stage.AddScreen(new Level10Screen(new SymbolImport("gameScreens", "level10Screen")));   //4 big wheel
            stage.AddScreen(new Level8Screen(new SymbolImport("gameScreens", "level8Screen")));     //4 roller balls
            stage.AddScreen(new Level0Screen(new SymbolImport("gameScreens", "level0Screen")));     //4 crusher
            stage.AddScreen(new Level3Screen(new SymbolImport("gameScreens", "level3Screen")));     //3 trampoline  slots
            stage.AddScreen(new Level11Screen(new SymbolImport("gameScreens", "level11ScreenB"), 1));   //3 piano B
            stage.AddScreen(new Level5Screen(new SymbolImport("gameScreens", "level5Screen")));     //4 seesaw anvil
            stage.AddScreen(new Level6Screen(new SymbolImport("gameScreens", "level6Screen")));     //3 two trampolines
            stage.AddScreen(new Level9Screen(new SymbolImport("gameScreens", "level9Screen")));     //3 ski jump
            stage.AddScreen(new Level11Screen(new SymbolImport("gameScreens", "level11ScreenC"), 2));   //4 piano C
            stage.AddScreen(new Level4Screen(new SymbolImport("gameScreens", "level4Screen")));     //5 last level
        }
        public override void ExitToMainMenu()
        {
            base.ExitToMainMenu();
            stage.SetScreen("titleScreens");
        }
        public void StartMusic()
        {
            dq = stage.music.PlaySound(Sfx.drums);
            bq = stage.music.PlaySound(Sfx.bass);
            introComplete = true;
        }
        public static bool introComplete = false;
		protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);

            if (introComplete)
            {
                if (dq.IsStopped)
                {
                    dq = stage.music.PlaySound(Sfx.drums);
                }
                if (bq.IsStopped)
                {
                    bq = stage.music.PlaySound(Sfx.bass);
                }
            }

#if !XBOX
            //KeyboardState ks = Keyboard.GetState();
            //if (!keyDown && ks.IsKeyDown(Keys.Left))
            //{
            //    keyDown = true;
            //    stage.PreviousScreen();
            //}
            //else if (!keyDown && ks.IsKeyDown(Keys.Right))
            //{
            //    keyDown = true;
            //    stage.NextScreen();
            //}
            //else if (keyDown && (ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Right)))
            //{
            //    keyDown = false;
            //}
#endif
		}
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            //GraphicsDevice.RenderState.DepthBufferEnable = true;
        }
	}
}
