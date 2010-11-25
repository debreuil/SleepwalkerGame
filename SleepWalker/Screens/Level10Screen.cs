
using DDW.V2D;
using Microsoft.Xna.Framework;
using Sleepwalker.Components;
using V2DRuntime.Attributes;
using V2DRuntime.Enums;
using V2DRuntime.V2D;
using Sleepwalker.Screens;
using Box2D.XNA;
using System;
using Sleepwalker.audio;

namespace Sleepwalker.Screens
{
    [V2DScreenAttribute(backgroundColor = 0x000001, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Level10Screen : BaseScreen
    {
        public override string Title { get { return "Big Wheel Keep on Turnin"; } }
        public override int Difficutly { get { return 4; } }

        public Teleporter teleporter;

        [V2DSpriteAttribute(isStatic = true, friction = .1f)]
        public V2DSprite[] staticBlock;

        [V2DSpriteAttribute(isSensor = true)]
        public TeleportDoor[] teleport;

        public Seesaw seesaw;
        public PressButton[] pressButton;

        [V2DSpriteAttribute(allowSleep = false)]
        public V2DSprite[] lift;


        [V2DSpriteAttribute(fixedRotation=false, friction=0.5f, allowSleep=false)]
        public V2DSprite[] blocker;

        [PrismaticJointAttribute(enableLimit = true, enableMotor = false, maxMotorForce = 10000)]
        public PrismaticJoint[] pj;

        [V2DSpriteAttribute(allowSleep = false, friction=0f, angularDamping=1.5f)]
        public TeleportDoor bigCircle;
        [RevoluteJointAttribute(enableLimit = true, enableMotor = false, maxMotorTorque = 1000000, collideConnected=false)]
        public RevoluteJoint rCircle;

        [RevoluteJointAttribute(collideConnected = false)]
        public RevoluteJoint[] rj;
        [RevoluteJointAttribute(collideConnected = false)]
        public RevoluteJoint[] rDrop;

        private int teleportIndex = -1;
        private bool toggleElevator = false;

        public Level10Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level10Screen(SymbolImport si)
            : base(si)
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            //walker.SetFriction(2f);
        }
        public override void Activate()
        {
            base.Activate();
            ClearBoundsBody(EdgeName.Top);
            ClearBoundsBody(EdgeName.Bottom);
            followPlayer = true;
            targetZoom = 1.25f;
            Camera.ShakeAmount = .2f;
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            //trampoline[0].tramp.body.GetFixtureList().SetRestitution(2.5f);
            //trampoline[1].tramp.body.GetFixtureList().SetRestitution(2.2f);
            //trampoline[2].tramp.body.GetFixtureList().SetRestitution(2.5f);
            //trampoline[3].tramp.body.GetFixtureList().SetRestitution(2.1f);
            //for (int i = 0; i < pj.Length; i++)
            //{
            //    pj[i]._enableMotor = true;
            //    pj[i]._motorSpeed = -slowLift;                
            //}
            toggleElevator = false;

            teleporter.node[0] = teleport[0];
            teleporter.node[2] = teleport[2];
            teleporter.node[4] = teleport[4];
            bigCircle.Index = 6;
            teleporter.node[6] = bigCircle;

            pj[0]._enableMotor = true;
            pj[0]._motorSpeed = fastLift;
            pj[2]._enableMotor = true;
            pj[2]._motorSpeed = slowLift;

            EnableMotorSound(false);
            //rCircle._motorMass = 1f;
        }

        public void EnableMotorSound(bool enable)
        {
            if (stage != null)
            {
                if (enable)
                {
                    if (!stage.audio.IsPlaying(Sfx.metalRollLoop))
                    {
                        stage.audio.PlaySound(Sfx.metalRollLoop);
                    }
                }
                else
                {
                    stage.audio.StopSound(Sfx.metalRollLoop);
                }
            }
        }

        const int slowLift = 6;
        const int fastLift = 20;
        private bool blowBeams = false;
        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is PressButton)
            {
                PressButton p = (PressButton)objB;
                if(!p.IsDepressed)
                {
                    p.IsDepressed = true;
                    switch(p.Index)
                    {
                        case 0:
                            pj[1]._enableMotor = true;
                            pj[1]._motorSpeed = -slowLift;
                            break;
                        case 1:
                            pj[2]._enableMotor = true;
                            pj[2]._motorSpeed = slowLift;
                            toggleElevator = true;
                            pressButton[5].IsDepressed = false;
                            break;
                        case 2:
                            pj[5]._enableMotor = true;
                            pj[5]._motorSpeed = fastLift;
                            pj[6]._enableMotor = true;
                            pj[6]._motorSpeed = fastLift;
                            break;
                        case 3:
                            pj[3]._enableMotor = true;
                            pj[3]._motorSpeed = -slowLift;
                            break;
                        case 4:
                            rCircle._enableLimit = false;
                            rCircle._enableMotor = true;
                            rCircle._motorSpeed = -1;
                            EnableMotorSound(true);
                            break;
                        case 5:
                            pj[2]._enableLimit = true;
                            pj[2]._enableMotor = true;
                            pj[2]._motorSpeed = -slowLift;
                            pressButton[1].IsDepressed = false;
                            break;
                    }
                }
            }
            else if (objB is TeleportDoor)
            {
                TeleportDoor td = (TeleportDoor)objB;
                if (td.Index % 2 == 0)
                {
                    teleportIndex = td.Index;
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

            for (int i = 0; i <= 0; i++)
            {
                if (toggleElevator && pj[i]._enableMotor)
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

            //if (rCircle._enableMotor && bigCircle.body.GetAngularVelocity() > -2)
            //{
            //    bigCircle.body.ApplyImpulse(new Vector2(50, 0), new Vector2(100, 100));
            //}
            //else
            //{
            //    bigCircle.body.ApplyImpulse(new Vector2(-25, 0), new Vector2(100, 100));
            //}
        }

        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            if (blowBeams)
            {
                blowBeams = false;
                world.DestroyJoint(rDrop[1]); 
                world.DestroyJoint(rDrop[2]);  
            }

            if (teleportIndex != -1)
            {
                teleporter.Teleport(walker, teleportIndex);
                teleportIndex = -1;
            }
        }

        public override void Deactivate()
        {
            EnableMotorSound(false);
            base.Deactivate();
        }

    }
}
