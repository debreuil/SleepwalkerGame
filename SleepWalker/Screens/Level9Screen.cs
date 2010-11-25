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
    public class Level9Screen : BaseScreen
    {
        public override string Title { get { return "Fly Like an Eagle"; } }
        public override int Difficutly { get { return 3; } }
        
        public Teleporter teleporter;
        public Trampoline[] trampoline;
        public PressButton[] pressButton;

        [V2DSpriteAttribute(allowSleep = false, friction = .5f)]
        public V2DSprite[] lift;
        [V2DSpriteAttribute(fixedRotation = false, friction = 0.5f, allowSleep = false)]
        public V2DSprite[] roller;

        [PrismaticJointAttribute(enableLimit = true, enableMotor = false, maxMotorForce = 1000000)]
        public PrismaticJoint[] pj;

        public RevoluteJoint[] rSignal;

        bool needsTeleport = false;

        public Level9Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level9Screen(SymbolImport si)  : base(si)
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            //Camera.ShakeAmount = .2f;
            //walker.directionWalking = 1;
        }
        public override void Activate()
        {
            base.Activate();
            followPlayer = true;
            targetZoom = 1f;
            ClearBoundsBody(EdgeName.Top);
            ClearBoundsBody(EdgeName.Bottom);
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            pj[0].EnableMotor(true);
            pj[0]._motorSpeed = -fastLift;
            trampoline[1].tramp.body.GetFixtureList().SetRestitution(3.2f);
            contactTypes.Add(pressButton[0].GetType(), OnPressButtonContact);
            oscMotor = true;
        }
        protected void OnPressButtonContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            if (((V2DSprite)objB).InstanceName == "roller0")
            {
                signalHit = true;
            }
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
                            pj[0]._enableMotor = true;
                            pj[0]._motorSpeed = -slowLift;
                            oscMotor = false;
                            break;
                        case 1:
                            pj[0]._enableMotor = true;
                            pj[0]._motorSpeed = slowLift;
                            oscMotor = false;
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
        private bool oscMotor = true;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (oscMotor)
            {
                for (int i = 0; i <= 0; i++)
                {
                    if (pj[i]._enableMotor)
                    {
                        if (pj[i]._limitState == LimitState.AtLower)
                        {
                            pj[i]._motorSpeed = fastLift;
                        }
                        else if (pj[i]._limitState == LimitState.AtUpper)
                        {
                            pj[i]._motorSpeed = -fastLift;
                        }
                    }
                }
            }
        }
        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            if (signalHit)
            {
                signalHit = false;
                if (rSignal[0] != null)
                {
                    world.DestroyJoint(rSignal[0]);
                    rSignal[0] = null;
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
