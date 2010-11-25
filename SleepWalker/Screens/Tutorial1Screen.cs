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
using V2DRuntime.Enums;

namespace Sleepwalker.Screens
{

    [V2DScreenAttribute(backgroundColor = 0x397093, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Tutorial1Screen : BaseScreen
    {
        public override string Title { get { return "Using Switches"; } }
        public override int Difficutly { get { return 1; } }
        
        public RevoluteJoint[] rj;
        public PressButton[] pressButton;

        public Tutorial1Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Tutorial1Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        public override void Activate()
        {
            base.Activate(); 
            walker.directionWalking = 1;
            targetZoom = 1.5f;
            ClearBoundsBody(EdgeName.Bottom);
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
                world.DestroyJoint(rj[0]);
                world.DestroyJoint(rj[1]);
            }
        }
    }
}
