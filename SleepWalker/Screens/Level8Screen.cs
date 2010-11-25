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
    public class Level8Screen : BaseScreen
    {
        public override string Title { get { return "Roll Over Beethoven"; } }
        public override int Difficutly { get { return 4; } }
        
        public Teleporter teleporter;
        public Seesaw seesaw;
        public PressButton[] pressButton;

        [V2DSpriteAttribute(allowSleep = false)]
        public V2DSprite[] lift;

        [V2DSpriteAttribute(fixedRotation=false, friction=0.5f, allowSleep=false)]
        public V2DSprite[] roller;

        [PrismaticJointAttribute(enableLimit = true, enableMotor = false, maxMotorForce = 1000000)]
        public PrismaticJoint[] pj;

        public RevoluteJoint[] rSignal;
        public RevoluteJoint[] rBlock;
        public RevoluteJoint[] rCement;
        public RevoluteJoint rElevator;

        private int teleportIndex = -1;
        private int signalHit = -1;

        public Level8Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level8Screen(SymbolImport si)  : base(si)
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            //followPlayer = true;
            //Camera.Zoom = 1.2f;
            //Camera.ShakeAmount = .2f;
            //walker.SetFriction(2f);
        }
        public override void Activate()
        {
            base.Activate();
            ClearBoundsBody(EdgeName.Top);
            targetZoom = 1.5f;
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            //trampoline[0].tramp.body.GetFixtureList().SetRestitution(2.5f);
            //trampoline[1].tramp.body.GetFixtureList().SetRestitution(2.2f);
            //trampoline[2].tramp.body.GetFixtureList().SetRestitution(2.5f);
            //trampoline[3].tramp.body.GetFixtureList().SetRestitution(2.1f);
        }
        const int slowLift = 6;
        const int fastLift = 12;
        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is PressButton)
            {
                PressButton p = (PressButton)objB;
                if (!p.IsDepressed)
                {
                    p.IsDepressed = true;
                    signalHit = p.Index;
                }
            }
            else if (objB is TeleportDoor)
            {
                TeleportDoor td = (TeleportDoor)objB;
                if (td.Index % 2 == 0)
                {
                    teleportIndex = td.Index;
                    walker.directionWalking = -walker.directionWalking;
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
            //for (int i = 2; i <= 3; i++)
            //{
            //    if (pj[i]._enableMotor)
            //    {
            //        if (pj[i]._limitState == LimitState.AtLower)
            //        {
            //            pj[i]._motorSpeed = fastLift;signalHit
            //        }
            //        else if (pj[i]._limitState == LimitState.AtUpper)
            //        {
            //            pj[i]._motorSpeed = -fastLift;
            //        }
            //    }
			 
            //}
        }

        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            if (signalHit != -1)
            {
                if (signalHit == 4)
                {
                    for (int i = 0; i < rCement.Length; i++)
                    {
                        world.DestroyJoint(rCement[i]);                        
                    }
                }
                else if (signalHit == 5)
                {
                    pj[0]._enableMotor = true;
                    pj[0]._motorSpeed = -slowLift;
                    world.DestroyJoint(rElevator);
                }
                else if(signalHit < rSignal.Length )
                {
                    world.DestroyJoint(rSignal[signalHit]);
                    world.DestroyJoint(rBlock[signalHit]);
                    rSignal[signalHit] = null;
                }
                signalHit = -1;
            }

            if (teleportIndex != -1)
            {
                teleporter.Teleport(walker, teleportIndex);
                teleportIndex = -1;
            }
        }

    }
}
