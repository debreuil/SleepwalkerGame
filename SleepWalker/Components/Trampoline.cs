using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Attributes;
using Box2D.XNA;
using V2DRuntime.Enums;
using Sleepwalker.audio;

namespace Sleepwalker.Components
{
    public class Trampoline : V2DSprite
    {
        [V2DSpriteAttribute(isStatic = true, restitution = 2f)]
        public V2DSprite tramp;

        [V2DSpriteAttribute(isStatic = true)]
        public V2DSprite blocker;

        public Trampoline(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public void Bounce()
        {
            tramp.GotoAndPlay(1);
            tramp.PlayheadWrap += new V2DRuntime.Display.AnimationEvent(Trampoline_PlayheadWrap);
            if (stage != null)
            {
                stage.audio.PlaySound(Sfx.boing);
            }
        }
        public void BounceSmall()
        {
            tramp.GotoAndPlay(2);
            tramp.PlayheadWrap += new V2DRuntime.Display.AnimationEvent(Trampoline_PlayheadWrap);
            if (stage != null)
            {
                stage.audio.PlaySound(Sfx.boingSmall);
            }
        }

        void Trampoline_PlayheadWrap(DDW.Display.DisplayObjectContainer sender)
        {
            tramp.GotoAndStop(0);
            tramp.PlayheadWrap -= new V2DRuntime.Display.AnimationEvent(Trampoline_PlayheadWrap);  
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Removed(System.EventArgs e)
        {
            base.Removed(e);
            tramp.Stop();
            tramp.PlayheadWrap -= new V2DRuntime.Display.AnimationEvent(Trampoline_PlayheadWrap);  
        }
    }
}
