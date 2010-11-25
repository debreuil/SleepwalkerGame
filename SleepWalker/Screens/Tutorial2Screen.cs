using System.Collections.Generic;
using DDW.Display;
using DDW.V2D;
using V2DRuntime.Attributes;
using V2DRuntime.V2D;
using Box2D.XNA;
using DDW.Input;
using Microsoft.Xna.Framework.Input;
using Sleepwalker.Components;
using System;
using Microsoft.Xna.Framework;

namespace Sleepwalker.Screens
{

    [V2DScreenAttribute(backgroundColor = 0x397093, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Tutorial2Screen : BaseScreen
    {
        public override string Title { get { return "Resetting Levels"; } }
        public override int Difficutly { get { return 1; } }
        
        public static bool isFirstTime = true;

        [PrismaticJointAttribute(enableLimit = true, enableMotor = true, maxMotorForce = 10000)]
        public PrismaticJoint[] pj;

        public PressButton[] pressButton;

        public Tutorial2Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Tutorial2Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        public override void Initialize()
        {
            base.Initialize(); 

            if (!isFirstTime)
            {
                pj[0].EnableMotor(true);
                pj[0]._motorSpeed = 12;
            }
            else
            {
                pj[0].EnableMotor(true);
                pj[0]._motorSpeed = -12;
            }
            isFirstTime = !isFirstTime;
        }
        public override void Activate()
        {
            base.Activate(); 
            walker.directionWalking = -1;
            targetZoom = 1.5f;
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
        }

        bool destroyJoint = false;
        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is PressButton)
            {
                PressButton pb = (PressButton)objB;
                if (!pb.IsDepressed)
                {
                    pb.IsDepressed = true;
                    destroyJoint = true;
                }
            }
        }

        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);
            if (destroyJoint)
            {
                destroyJoint = false;
            }
        }
    }
}
