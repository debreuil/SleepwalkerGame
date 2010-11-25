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
    public class Level4Screen : BaseScreen
    {
        public override string Title { get { return "Stairway to Heaven"; } }
        public override int Difficutly { get { return 5; } }
        
        public Teleporter teleporter;

        public TiltElevator[] elevator;
        public Trampoline[] trampoline;

        bool needsTeleport = false;


        public Level4Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level4Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        public override void Activate()
        {
            base.Activate();
            isFinalLevel = true;
            followPlayer = true;
            Camera.clampY = true;
            targetZoom = 1.4f;
            Camera.ShakeAmount = .2f;
            ClearBoundsBody(EdgeName.Top);
        }

        //int[] speeds = new int[] { 8, 9, 12, 8, 6, 10, 6, 5, 9, 7, 10, 11, 8, 13, 10 };
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            for (int i = 0; i < elevator.Length; i++)
            {
                elevator[i].Speed = 8;// speeds[i];
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
                    needsTeleport = true;
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

            if (needsTeleport)
            {
                needsTeleport = false;
                walker.body.SetLinearVelocity(Vector2.Zero);
                walker.body.Position = teleporter.node[1].body.Position;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
