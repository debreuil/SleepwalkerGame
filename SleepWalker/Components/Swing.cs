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
    public class Swing : V2DSprite
    {
        [V2DSpriteAttribute(isSensor = true)]
        public List<V2DSprite> rope;
        [V2DSpriteAttribute(isStatic = true)]
        public V2DSprite anchor;
        [V2DSpriteAttribute()]
        public V2DSprite beam;
        [RevoluteJointAttribute()]
        public List<RevoluteJoint> ropeRj;

        public Swing(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
