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
    public class Level3Screen : BaseScreen
    {
        public override string Title { get { return "The Wall Part III"; } }
        public override int Difficutly { get { return 3; } }

        [V2DSpriteAttribute(isStatic = true, friction = 0f)]
        public V2DSprite[] barrier;

        public Trampoline[] trampoline;

        public Teleporter teleporter;
        public TiltPole[] tiltPole;

        bool needsTeleport = false;

        public Level3Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level3Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            ClearBoundsBody(EdgeName.Top);

            if (needsTeleport)
            {
                needsTeleport = false;
                walker.body.SetLinearVelocity(Vector2.Zero);
                walker.body.Position = teleporter.node[1].body.Position;
            }
        }

        public override void Activate()
        {
            base.Activate();
            targetZoom = 1.5f;
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
            else if (objB is V2DSprite)
            {
                if (((V2DSprite)objB).Parent is Trampoline)
                {
                    Walker p = (Walker)player;
                    if (p.Visible)
                    {
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
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
