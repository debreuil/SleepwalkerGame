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
    public class TrapDoor : V2DSprite
    {
        public V2DSprite[] item;
        [V2DSpriteAttribute(isStatic = true)]
        public V2DSprite[] guide;
        public V2DSprite door;
        public RevoluteJoint[] rj;

        public TrapDoor(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public void OpenDoor()
        {
            if (rj[0] != null)
            {
                ((V2DScreen)screen).world.DestroyJoint(rj[0]);
                rj[0] = null;

                if (stage != null)
                {
                    stage.audio.PlaySound(Sfx.creakDoor);
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
