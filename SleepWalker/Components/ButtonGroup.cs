using System;
using System.Collections.Generic;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;
using V2DRuntime.Components;
using V2DRuntime.Attributes;
using Box2D.XNA;

namespace Sleepwalker.Components
{
    public class ButtonGroup : V2DSprite
    {
        [V2DSpriteAttribute()]
        public PressButton[] pressButton;

        public ButtonGroup(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        private int selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                for (int i = 0; i < pressButton.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        pressButton[i].IsDepressed = true;
                    }
                    else
                    {
                        pressButton[i].IsDepressed = false;
                    }
                    
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
