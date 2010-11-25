using System;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Sleepwalker.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;
using DDW.Display;
using Sleepwalker.Components;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace Sleepwalker.Panels
{
	public class ChooseLevelPanel : Panel
	{
        public LevelDescription levelDescription;
        public Sprite fullVersionBlurb;

		public ChooseLevelPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        public override void Activate()
        {
            base.Activate();
            fullVersionBlurb.Visible = Guide.IsTrialMode ? true : false;
        }
        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
        {
            return base.OnPlayerInput(playerIndex, move, time);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (levelDescription.levelSelected)
            {
                levelDescription.levelSelected = false;
                if (!Guide.IsTrialMode)
                {
                    stage.SetScreen(LevelDescription.focusIndex + 1);
                }
            }

            // catch when purchased
            if(!Guide.IsTrialMode && fullVersionBlurb.Visible)
            {
                fullVersionBlurb.Visible = false;
            }
        }
	}
}
