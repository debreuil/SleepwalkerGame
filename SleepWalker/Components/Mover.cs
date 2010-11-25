using System;
using System.Collections.Generic;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;
using V2DRuntime.Components;
using V2DRuntime.Attributes;
using Box2D.XNA;
using Microsoft.Xna.Framework.Audio;
using Sleepwalker.audio;

namespace Sleepwalker.Components
{
    public class Mover : V2DSprite
    {
        [V2DSpriteAttribute(isStatic = true)]
        public V2DSprite anchor;
        [V2DSpriteAttribute(allowSleep = false)]
        public V2DSprite beam;
        [PrismaticJointAttribute(enableLimit = true, enableMotor = true, motorSpeed = -15, maxMotorForce = 5000)]
        public PrismaticJoint pj;

        public MotorTracker motorTracker;
        private int speed;

        public Mover(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public int Speed { 
            get { return speed; }
            set
            {
                speed = value;
                pj._motorSpeed = speed;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (motorTracker != null)
            {
                motorTracker.UpdateSound();
            }
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            if (pj != null && stage != null)
            {
                motorTracker = new MotorTracker(stage.audio, pj, Sfx.rollUpChainLoop);
            }
        }
        public override void RemovedFromStage(EventArgs e)
        {
            if (motorTracker != null)
            {
                motorTracker.StopAllSounds();
            }
            base.RemovedFromStage(e);
        }
    }
}
