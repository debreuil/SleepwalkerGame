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
using Microsoft.Xna.Framework;

namespace Sleepwalker.Components
{
    public class Teleporter : V2DSprite
    {
        [V2DSpriteAttribute(isStatic = true, isSensor = true)]
        public TeleportDoor[] node;

        public Teleporter(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public void Teleport(Walker walker, int nodeIndex)
        {
            int targetNode = (node.Length > nodeIndex + 1) ? nodeIndex + 1 : 0;

            if (stage != null)
            {
                stage.audio.PlaySound(Sfx.teleport2);
            }
            walker.body.SetLinearVelocity(Vector2.Zero);
            walker.body.Position = node[targetNode].body.Position;
            if (node[nodeIndex].FrameCount > 1 && node[targetNode].FrameCount > 1)
            {
                node[nodeIndex].GotoAndPlay(1);
                node[targetNode].GotoAndPlay(1);
                node[nodeIndex].PlayheadWrap += new V2DRuntime.Display.AnimationEvent(Teleporter_PlayheadWrap);
            }
        }

        void Teleporter_PlayheadWrap(DisplayObjectContainer sender)
        {
            this.PlayheadWrap -= new V2DRuntime.Display.AnimationEvent(Teleporter_PlayheadWrap);
            for (int i = 0; i < node.Length; i++)
            {
                node[i].GotoAndStop(0);                
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
