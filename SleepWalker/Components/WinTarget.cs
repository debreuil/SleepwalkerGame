using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Attributes;
using Box2D.XNA;

namespace Sleepwalker.Components
{
    public class WinTarget : V2DSprite
    {
        public WinTarget(Texture2D texture, V2DInstance instance) : base(texture, instance)
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
