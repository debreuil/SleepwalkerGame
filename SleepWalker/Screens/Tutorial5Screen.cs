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
    public class Tutorial5Screen : BaseScreen
    {
        public override string Title { get { return "Puzzle Solving"; } }
        public override int Difficutly { get { return 1; } }
        
        public RevoluteJoint[] rBlocker;
        public PressButton[] pressButton;
        public Seesaw seesaw;

        [V2DSpriteAttribute(friction=0.5f)]
        public V2DSprite blocker;

        public Tutorial5Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Tutorial5Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        public override void Initialize()
        {
            base.Initialize(); 
        }
        public override void Activate()
        {
            base.Activate();
            walker.directionWalking = -1;
            followPlayer = false;
            targetZoom = 1.25f;
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            contactTypes.Add(pressButton[0].GetType(), OnPressButtonContact);
            targetZoom = 1;
            followPlayer = false;
        }

        protected void OnPressButtonContact(object pressButton, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            if (objB is V2DSprite && ((V2DSprite)objB).InstanceName == "item2")
            {
                PressButton pb = (PressButton)pressButton;
                if (!pb.IsDepressed)
                {
                    pb.IsDepressed = true;
                    if (pb.Index == 1)
                    {
                        dropBeam = true;
                    }
                }
            }
        }
        bool openDoor = false;
        bool dropBeam = false;
        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is PressButton)
            {
                PressButton pb = (PressButton)objB;
                if (!pb.IsDepressed)
                {
                    pb.IsDepressed = true;
                    if (pb.Index == 0)
                    {
                        openDoor = true;
                    }
                }
            }
        }

        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);
            if (openDoor)
            {
                openDoor = false;
                world.DestroyJoint(rBlocker[2]);
            }

            if (dropBeam)
            {
                dropBeam = false;
                world.DestroyJoint(rBlocker[1]);
            }
        }
    }
}
