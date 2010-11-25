
using System;
using System.Collections.Generic;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using V2DRuntime.Display;
using V2DRuntime.Components;
using System.Globalization;

namespace Sleepwalker.Panels
{
    public class EndRoundPanel : Panel
    {
        public TextBox txNextLevelName;
        public V2DSprite[] difficultyIcon;

        public EndRoundPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
		{			
			bool result = base.OnPlayerInput(playerIndex, move, time);
			if (result && isActive)
			{
				if ((move.Releases & Buttons.A) != 0) // == Move.ButtonA)
				{
                    Continue(this, null);
                    result = false;
				}
			}
			return result;
		}
        public void SetStats(string title, int difficulty)
        {            
            if(txNextLevelName != null)
            {
                txNextLevelName.Text = title.ToUpper(CultureInfo.InvariantCulture);
                for (int i = 0; i < difficultyIcon.Length; i++)
                {
                    difficultyIcon[i].GotoAndStop(i < difficulty ? 1u : 0u);
                }
            }  
        }
        public override void Removed(EventArgs e)
        {
            base.Removed(e);
        }
		public event EventHandler Continue;
    }
}
