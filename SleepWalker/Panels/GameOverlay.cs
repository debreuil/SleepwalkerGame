using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Attributes;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using DDW.Display;
using Sleepwalker;
using Sleepwalker.Screens;
using Microsoft.Xna.Framework;

namespace Sleepwalker.Panels
{
    public class GameOverlay : Sprite
    {
        [SpriteAttribute(depthGroup = 1000)]
        public SignInPanel signInPanel;
        [SpriteAttribute(depthGroup = 1000)]
        public PausedPanel pausedPanel;
        [SpriteAttribute(depthGroup = 1000)]
        public EndRoundPanel endRoundPanel;
        
        public bool hasActivePanel = false;

        public GameOverlay(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }

        public override void Initialize()
        {
            base.Initialize();

            hasActivePanel = false;

            pausedPanel.Unpause += new EventHandler(pausedPanel_Unpause);
            pausedPanel.Deactivate();

            endRoundPanel.Continue += new EventHandler(endRoundPanel_Continue);
            endRoundPanel.Deactivate();

            signInPanel.Unpause += new EventHandler(signInPanel_Unpause);
            signInPanel.Deactivate();
        }

        public void SignInToLive()
        {
            SetCamera();
            signInPanel.Activate();
            //stage.audio.PauseSound(Sfx.backgroundMusic);
            hasActivePanel = true;
        }
        public void PauseGame()
        {
            SetCamera();
            pausedPanel.Activate();
            //stage.audio.PauseSound(Sfx.backgroundMusic);
            hasActivePanel = true;
        }
        protected void pausedPanel_Unpause(object sender, EventArgs e)
        {
            RestoreCamera();
            pausedPanel.Deactivate();
            //stage.audio.ResumeSound(Sfx.backgroundMusic);
            hasActivePanel = false;
        }
        protected void signInPanel_Unpause(object sender, EventArgs e)
        {
            RestoreCamera();
            signInPanel.Deactivate();
            //stage.audio.ResumeSound(Sfx.backgroundMusic);
            hasActivePanel = false;
        }

        public void EndRound()
        {

            if(screen.isFinalLevel)
            {
                endRoundPanel.GotoAndStop(1u);
            }
            else
            {
                endRoundPanel.GotoAndStop(0u);
                if (stage != null && stage.GetNextScreen() is BaseScreen)
                {
                    BaseScreen nscr = (BaseScreen)stage.GetNextScreen();
                    endRoundPanel.SetStats(nscr.Title, nscr.Difficutly);
                }
            }
            SetCamera();
            endRoundPanel.Activate();
            //stage.audio.PauseSound(Sfx.backgroundMusic);
            hasActivePanel = true;
        }
        void endRoundPanel_Continue(object sender, EventArgs e)
        {
            RestoreCamera();
            endRoundPanel.Deactivate();
            stage.NextScreen();
            hasActivePanel = false;
            ((BaseScreen)screen).skipLevel = true;
        }

        //bool prevFollowPlayer = false;
        //float prevCameraZoom = 1f;
        private void SetCamera()
        {
            if(parent is BaseScreen)
            {
                BaseScreen bs = (BaseScreen)parent;

                Vector3 sc, tr;
                Quaternion r;
                DDW.Display.Stage.SpriteBatchMatrix.Decompose(out sc, out r, out tr);
                this.Scale = new Vector2(1f/sc.X, 1f/sc.Y);
                this.Position = new Vector2(-tr.X, -tr.Y) * Scale; 
            }
            //prevFollowPlayer = ((BaseScreen)screen).followPlayer;
            //prevCameraZoom = ((BaseScreen)screen).Camera.Zoom;
            //((BaseScreen)screen).followPlayer = false;
            //((BaseScreen)screen).Camera.Zoom = 1.25f;
        }
        private void RestoreCamera()
        {
            //((BaseScreen)screen).followPlayer = prevFollowPlayer;
            //((BaseScreen)screen).Camera.Zoom = prevCameraZoom;
        }
        protected override void SetCurrentState()
        {
            base.SetCurrentState();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (pausedPanel.isActive && pausedPanel.needsExitToMain)
            {
                pausedPanel.needsExitToMain = false;
                pausedPanel_Unpause(this, null);
                ((BaseScreen)screen).ExitToMainMenu();
            }
        }
    }
}
