using System;
using System.Collections.Generic;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;
using V2DRuntime.Components;
using V2DRuntime.Attributes;
using Box2D.XNA;
using Sleepwalker.audio;

namespace Sleepwalker.Components
{
    public class PressButton : V2DSprite
    {
        private bool isDepressed = false;
        public PressButton(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public bool IsDepressed 
        { 
            get{return isDepressed;}
            set
            {
                isDepressed = value; 
                GotoAndStop(value ? 1u : 0u);
                if (stage != null)
                {
                    if (value)
                    {
                        stage.audio.PlaySound(Sfx.pressButton);
                    }
                    else
                    {
                        stage.audio.PlaySound(Sfx.switchOn);
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
