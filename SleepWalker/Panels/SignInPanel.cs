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
	public class SignInPanel : Panel
	{
		public ButtonTabGroup menuButtons;

        public SignInPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
		{			
			bool result = base.OnPlayerInput(playerIndex, move, time);
			if (result && isActive)
			{
				if (move == Move.Start)
				{
                    DoUnpause();
					result = false;
				}
			}
			return result;
		}
		void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		{
			switch (sender.Index)
			{
				case 0:
                    DoUnpause();
                    SleepwalkerGame.instance.ShowSignIn();
					break;
				case 1:
                    DoUnpause();
					break;
			}
		}
        private void DoUnpause()
        {
            if (Unpause != null)
            {
                Unpause(this, null);
            }
        }
		public override void Activate()
		{
            base.Activate();

            menuButtons.wrapAround = true;

			menuButtons.SetFocus(0);
			menuButtons.OnClick += new ButtonEventHandler(menuButtons_OnClick);
		}
		public override void Deactivate()
		{
			base.Deactivate();
			menuButtons.OnClick -= new ButtonEventHandler(menuButtons_OnClick);
        }
		public event EventHandler Unpause;
	}
}
