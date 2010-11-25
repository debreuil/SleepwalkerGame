using System;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Sleepwalker.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;
using DDW.Input;
using DDW.Display;
using Microsoft.Xna.Framework.GamerServices;

namespace Sleepwalker.Panels
{
	public class ExitPanel : Panel
	{
        protected Sprite pitchPages;
        protected Sprite upgradeTrial;

		public ExitPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
        {
			bool result = base.OnPlayerInput(playerIndex, move, time);

            if (result && isActive)
            {
                if (move == Move.ButtonA)
                {
                    NextPitch();
                    result = false;
                }
                else if (move == Move.ButtonX)
                {
                    SleepwalkerGame.instance.UnlockTrial();
                    result = false;
                }
            }
            return result;
        }

        private void NextPitch()
        {
            if (pitchPages.CurChildFrame + 1 < pitchPages.FrameCount)
            {
                pitchPages.GotoAndStop(pitchPages.CurChildFrame + 1);
            }
            else
            {
                SleepwalkerGame.instance.ExitGame();
            }
        }

        public override void Activate()
        {
            base.Activate();

            if (Guide.IsTrialMode)
            {
                pitchPages.GotoAndStop(0);
            }
            else
            {
                pitchPages.GotoAndStop(2);
                upgradeTrial.Visible = false;
            }
        }
        public override void Deactivate()
        {
            base.Deactivate();
        }

	}
}
