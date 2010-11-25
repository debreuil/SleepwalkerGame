using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Attributes;
using Box2D.XNA;

namespace Sleepwalker.Components
{
    public class TiltPole : V2DSprite
    {
        [RevoluteJointAttribute(lowerAngle = -.2f, upperAngle = .2f, enableLimit = true)]
        public RevoluteJoint rj;

        [V2DSpriteAttribute(isStatic = true, isSensor = true)]
        public V2DSprite[] buildBeam;

        [V2DSpriteAttribute(isSensor = true)]
        public V2DSprite[] invBeam;

        [V2DSpriteAttribute()]
        public V2DSprite seesawBeam;


        public TiltPole(Texture2D texture, V2DInstance instance) : base(texture, instance)
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
