using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Attributes;
using Box2D.XNA;
using SharpNeatLib.Maths;
using System;

namespace Sleepwalker.Components
{
    public class TiltElevator : V2DSprite
    {
        [RevoluteJointAttribute(lowerAngle = -.1f, upperAngle = .1f, enableLimit = true)]
        public RevoluteJoint rj;
        [RevoluteJointAttribute(lowerAngle = -.1f, upperAngle = .1f, enableLimit = true)]
        public RevoluteJoint rt;

        [PrismaticJointAttribute(enableMotor = true, maxMotorForce = 80000, motorSpeed = -5)]
        public PrismaticJoint pj;

        [V2DSpriteAttribute(isSensor = true)]
        public V2DSprite[] buildBeam;

        [V2DSpriteAttribute(isSensor = true)]
        public V2DSprite[] invBeam;

        [V2DSpriteAttribute()]
        public V2DSprite seesawBeam;


        private int speed = 5;
        private int direction = 1;
        private static Random rnd = new Random((int)DateTime.Now.Ticks);


        public TiltElevator(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }

        public int Speed { 
            get { return speed; } 
            set 
            {
                speed = value; 
                direction = rnd.Next(2) > .5 ? 1 : -1;
                pj.SetMotorSpeed(speed * direction);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (pj != null)
            {
                if (pj._limitState == LimitState.AtUpper && direction == 1)
                {
                    direction = -1;
                    pj.SetMotorSpeed(speed * direction);
                }
                else if (pj._limitState == LimitState.AtLower && direction == -1)
                {
                    direction = 1;
                    pj.SetMotorSpeed(speed * direction);
                }
            }
        }
    }
}

