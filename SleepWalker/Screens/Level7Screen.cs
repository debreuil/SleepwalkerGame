using DDW.V2D;
using Microsoft.Xna.Framework;
using Sleepwalker.Components;
using V2DRuntime.Attributes;
using V2DRuntime.Enums;
using V2DRuntime.V2D;
using Sleepwalker.Screens;
using Box2D.XNA;

namespace Sleepwalker.Screens
{
    [V2DScreenAttribute(backgroundColor = 0x000001, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Level7Screen : BaseScreen
    {
        public override string Title { get { return "Leap of Faith"; } }
        public override int Difficutly { get { return 3; } }

        [V2DSpriteAttribute(isStatic = true, friction = 0f)]
        public new V2DSprite[] beam;

        public Teleporter teleporter;
        public Trampoline[] trampoline;
        public PressButton[] pressButton;

        [V2DSpriteAttribute(allowSleep = false)]
        public V2DSprite[] lift;

        [PrismaticJointAttribute(enableLimit = true, enableMotor = false, maxMotorForce = 1000000)]
        public PrismaticJoint[] pj;

        public RevoluteJoint rSignal;

        bool needsTeleport = false;

        public Level7Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level7Screen(SymbolImport si)  : base(si)
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            targetZoom = 1.5f;
        }
        public override void Activate()
        {
            base.Activate();
            walker.directionWalking = 1;
            ClearBoundsBody(EdgeName.Top);
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            trampoline[0].tramp.body.GetFixtureList().SetRestitution(2.5f);
            trampoline[1].tramp.body.GetFixtureList().SetRestitution(2.2f);
            trampoline[2].tramp.body.GetFixtureList().SetRestitution(2.5f);
            trampoline[3].tramp.body.GetFixtureList().SetRestitution(2.1f);
        }
        const int slowLift = 6;
        const int fastLift = 12;
        private bool signalHit = false;
        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is PressButton)
            {
                PressButton p = (PressButton)objB;
                if (!p.IsDepressed)
                {
                    p.IsDepressed = true;
                    switch (p.Index)
                    {
                        case 0:
                            signalHit = true;
                            pj[0]._enableMotor = true;
                            pj[0]._motorSpeed = -slowLift;
                            break;
                    }
                }
            }
            else if (objB is TeleportDoor)
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
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (pj[0]._enableMotor)
            {
                if (pj[0]._limitState == LimitState.AtLower)
                {
                    pj[0]._motorSpeed = slowLift;
                }
                else if (pj[0]._limitState == LimitState.AtUpper)
                {
                    pj[0]._motorSpeed = -slowLift;
                }
            }
        }
        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            if (signalHit)
            {
                signalHit = false;
                if (rSignal != null)
                {
                    world.DestroyJoint(rSignal);
                    rSignal = null;
                }
            }

            if (needsTeleport)
            {
                needsTeleport = false;
                teleporter.Teleport(walker, 0);
            }
        }

    }
}
