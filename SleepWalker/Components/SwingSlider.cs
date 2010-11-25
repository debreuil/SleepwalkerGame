using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using V2DRuntime.Attributes;
using Box2D.XNA;
using Microsoft.Xna.Framework.Graphics;

namespace Sleepwalker.Components
{
    public class SwingSlider : Swing
    {
        [V2DSpriteAttribute(isStatic = false)]
        public new V2DSprite anchor;

        [PrismaticJointAttribute(enableMotor = false, maxMotorForce = 80000)]
        public PrismaticJoint pj;

        public SwingSlider(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public void SetMotorSpeed(float speed)
        {
            if (pj != null)
            {
                pj.SetMotorSpeed(speed);
                pj.EnableMotor(true);
            }
        }
    }
}
