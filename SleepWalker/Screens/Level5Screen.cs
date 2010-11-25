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
    [V2DScreenAttribute(backgroundColor = 0x000001, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Level5Screen : BaseScreen
    {
        public override string Title { get { return "Just a Little Patience"; } }
        public override int Difficutly { get { return 4; } }
        
        //public Teleporter teleporter;

        public TiltElevator[] elevator;
        public Trampoline[] trampoline;
        public Seesaw seesaw;
        public PressButton[] pressButton;
        public SwingSlider swing;

        [V2DSpriteAttribute(friction=.2f, angularDamping=3)]
        public V2DSprite[] weight;

        //bool needsTeleport = false;

        public Level5Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level5Screen(SymbolImport si)  : base(si)
        {
            SymbolImport = si;
        }
        public override void Activate()
        {
            base.Activate();
            walker.directionWalking = 1;
            ClearBoundsBody(EdgeName.Top);
            targetZoom = 1.15f;
            followPlayer = false;
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            contactTypes.Add(pressButton[0].GetType(), OnPressButtonContact);

            for (int i = 0; i < elevator.Length; i++)
            {
                elevator[i].Speed = 6;
            }
            trampoline[0].tramp.body.GetFixtureList().SetRestitution(1.1f);
        }
        protected void OnPressButtonContact(object pb, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            if (objB is V2DSprite && ((V2DSprite)objB).InstanceName == "weight0")
            {
                pressButton[0].IsDepressed = true;
                pressButton[1].IsDepressed = true;
                swing.SetMotorSpeed(5);
            }
        }
        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is TeleportDoor)
            {
                TeleportDoor td = (TeleportDoor)objB;
                if (td.Index == 0)
                {
                    //needsTeleport = true;
                }
            }
            else if (objB is V2DSprite && ((V2DSprite)objB).Parent is Trampoline)
            {
                Walker p = (Walker)player;

                float ls = p.body.GetLinearVelocity().LengthSquared();

                if (ls > 400f)
                {
                    ((Trampoline)((V2DSprite)objB).Parent).Bounce();
                }
                else if (ls > 200f)
                {
                    ((Trampoline)((V2DSprite)objB).Parent).BounceSmall();
                }

                if (ls > 1000f)
                {
                    p.body.SetLinearVelocity(p.body.GetLinearVelocity() * .8f);
                }
            }
        }

        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            //if (needsTeleport)
            //{
            //    needsTeleport = false;
            //    walker.body.SetLinearVelocity(Vector2.Zero);
            //    walker.body.Position = teleporter.node[1].body.Position;
            //}
        }

    }
}
