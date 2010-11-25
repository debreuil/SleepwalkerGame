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
    public class Smasher : V2DSprite
    {
        public V2DSprite crusherBack;
        public V2DSprite crusherFront;

        [V2DSpriteAttribute(categoryBits = 2, maskBits = 1)]
        public V2DSprite elevatorBeam;

        [V2DSpriteAttribute(isSensor = true, categoryBits = 2, maskBits = 1)]
        public V2DSprite[] arm;

        [PrismaticJointAttribute(enableLimit = true)]
        public PrismaticJoint pj;

        [RevoluteJointAttribute(enableMotor = false, maxMotorTorque = 80000, motorSpeed = -3)]
        public RevoluteJoint rb;

        public RevoluteJoint[] ar;

        private bool crushing = true;

        public Smasher(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
            rb._enableMotor = false;
        }
        public void EnableMotor(bool enable)
        {
            rb._enableMotor = enable;
            crushing = true;

            if (stage != null)
            {
                if (enable)
                {
                    if (!stage.audio.IsPlaying(Sfx.metalRollLoop))
                    {
                        stage.audio.PlaySound(Sfx.metalRollLoop);
                    }
                }
                else
                {
                    stage.audio.StopSound(Sfx.metalRollLoop);
                }
            }
        }
        public void SetMotorSpeed(int speed)
        {
            rb._motorSpeed = speed;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (rb._enableMotor)
            {
                if (elevatorBeam.Y > 60 && !crushing)
                {
                    crushing = true;
                    stage.audio.PlaySound(Sfx.crusher);
                }
                else if(elevatorBeam.Y < -0 )
                {
                    crushing = false;
                }
            }            
        }
        public override void Deactivate()
        {
            if (stage != null)
            {
                stage.audio.StopSound(Sfx.metalRollLoop);
                stage.audio.StopSound(Sfx.crusher);
            }
            base.Deactivate();
        }
    }
}
