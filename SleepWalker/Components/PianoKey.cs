
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Attributes;
using Box2D.XNA;
using Sleepwalker.audio;
using Microsoft.Xna.Framework.Audio;
using V2DRuntime.Audio;
using System;

namespace Sleepwalker.Components
{
    public class PianoKey : V2DSprite
    {
        private bool isNotePlaying = false;
        private int keyWait = 0;
        private int keyDelay = 100;

        public PianoKey(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public void PlayNote()
        {
            isNotePlaying = true;
            keyWait = 0;
            GotoAndStop(1);
            if (stage != null)
            {
                stage.audio.PlaySound(Sfx.note + Index, OnNoteEnd);
            }
        }

        public void OnNoteEnd(Cue c)
        {
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (isNotePlaying)
            {
                keyWait += gameTime.ElapsedGameTime.Milliseconds;
                if (keyWait >= keyDelay)
                {
                    GotoAndStop(0);
                    isNotePlaying = false;
                }
            }
        }
    }
}
