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
    public class Level6Screen : BaseScreen
    {
        public override string Title { get { return "Wall of Confusion"; } }
        public override int Difficutly { get { return 3; } }
        
        public Teleporter teleporter;
        public Trampoline[] trampoline;
        public PressButton[] pressButton;

        [V2DSpriteAttribute(allowSleep = false)]
        public V2DSprite[] lift;

        [PrismaticJointAttribute(enableLimit = true, enableMotor = false, maxMotorForce = 1000000)]
        public PrismaticJoint[] pj;

        bool needsTeleport = false;

        public Level6Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level6Screen(SymbolImport si)  : base(si)
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            //Camera.Zoom = 1.2f;
            //Camera.ShakeAmount = .2f;
            //walker.directionWalking = 1;
            targetZoom = 1.5f;
            followPlayer = true;
        }
        public override void Activate()
        {
            base.Activate();
            //ClearBoundsBody(EdgeName.Top);
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            for (int i = 0; i <= 1; i++)
            {
                pj[i]._enableMotor = true;
                pj[i]._motorSpeed = 6;
            }
            pj[1]._motorSpeed = -6;
            //trampoline[0].tramp.body.GetFixtureList().SetRestitution(1.1f);
        }
        const int slowLift = 6;
        const int fastLift = 12;
        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is PressButton)
            {
                PressButton p = (PressButton)objB;
                p.IsDepressed = true;
                switch (p.Index)
                {
                    case 0:
                        pj[0]._enableMotor = true;
                        pj[0]._motorSpeed = -slowLift;
                        pj[1]._motorSpeed = slowLift;
                        break;
                    case 1:
                        pj[1]._enableMotor = true;
                        pj[1]._motorSpeed = -slowLift;
                        break;
                    case 2:
                        pj[2]._enableMotor = true;
                        pj[2]._motorSpeed = -fastLift;
                        break;
                    case 3:
                        pj[3]._enableMotor = true;
                        pj[3]._motorSpeed = -fastLift;
                        break;
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
            for (int i = 2; i <= 3; i++)
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
        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);
            if (needsTeleport)
            {
                needsTeleport = false;
                teleporter.Teleport(walker, 0);
            }
        }

    }
}
