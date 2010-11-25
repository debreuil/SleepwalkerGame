
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using DDW.Input;
using DDW.V2D;
using V2DRuntime.Components;
using V2DRuntime.Display;
using Sleepwalker;

namespace Sleepwalker.Panels
{
	public class PausedPanel : Panel
	{
		public ButtonTabGroup menuButtons;
		private enum ExitButton { Back, Exit, UnlockTrial };

        public bool needsExitToMain = false;
		public PausedPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
		{			
			bool result = base.OnPlayerInput(playerIndex, move, time);
			if (result && isActive)
			{
				if (move == Move.Start)
				{
					Unpause(this, null);
					result = false;
				}
			}
			return result;
		}
		void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		{
			switch ((ExitButton)sender.Index)
			{
				case ExitButton.Exit:
                    needsExitToMain = true;
					break;
				case ExitButton.Back:
					Unpause(this, null);
					break;
				case ExitButton.UnlockTrial:
					Unpause(this, null);
                    SleepwalkerGame.instance.UnlockTrial();
					break;
			}
		}

		public override void Activate()
		{
            base.Activate();

            needsExitToMain = false;
            menuButtons.wrapAround = true;
            menuButtons.element[2].Visible = Guide.IsTrialMode;

			menuButtons.SetFocus(0);
			menuButtons.OnClick += new ButtonEventHandler(menuButtons_OnClick);
		}
		public override void Deactivate()
		{
			base.Deactivate();
			menuButtons.OnClick -= new ButtonEventHandler(menuButtons_OnClick);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            menuButtons.element[2].Visible = Guide.IsTrialMode;
        }
		public event EventHandler Unpause;
	}
}
