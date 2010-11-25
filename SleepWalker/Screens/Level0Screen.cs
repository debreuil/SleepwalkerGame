using System.Collections.Generic;
using DDW.Display;
using DDW.V2D;
using V2DRuntime.Attributes;
using V2DRuntime.V2D;
using DDW.Input;
using Microsoft.Xna.Framework.Input;
using Sleepwalker.Components;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using Microsoft.Xna.Framework;
using Sleepwalker.audio;

namespace Sleepwalker.Screens
{
    [V2DScreenAttribute(backgroundColor = 0x397093, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Level0Screen : BaseScreen
    {
        public override string Title { get { return "Heartbreaker"; } }
        public override int Difficutly { get { return 4; } }

        [RevoluteJointAttribute(enableMotor = false, maxMotorTorque = 50000, motorSpeed = 2)]
        public RevoluteJoint rcw;

        [RevoluteJointAttribute(enableMotor = false, maxMotorTorque = 50000, motorSpeed = -2)]
        public RevoluteJoint rccw;
        
        [V2DSpriteAttribute(restitution=.2f)]
        public V2DSprite[] hex;

        [V2DSpriteAttribute()]
        public V2DSprite[] rotationBeam;

        [V2DSpriteAttribute(isStatic = true)]
        public V2DSprite[] floor;

        public TrapDoor trapDoor;        
        public Smasher[] smasher;
        public PressButton[] panicButton;
        public Seesaw seesaw;

        bool openDoor = false;
        bool needsTeleport = false;
        Vector2 startPos;

        public Level0Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level0Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            //contactTypes.Add(seesaw.GetType(), OnSeesawContact);
        }
        public override void Activate()
        {
            base.Activate();

            followPlayer = true;
            targetZoom = 1.4f;
            Camera.ShakeAmount = .2f;

            startPos = walker.body.GetPosition();
            smasher[0].SetMotorSpeed(-3);
            smasher[1].SetMotorSpeed(4);
        }
        public override void DestroyView()
        {
            base.DestroyView();
            if (stage != null)
            {
                stage.audio.StopSound(Sfx.metalRollLoop);
            }
        }
        //protected void OnSeesawContact(object player, object objB)
        //{
        //    if(objB is
        //}
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
                        smasher[0].SetMotorSpeed(-3);
                        smasher[0].EnableMotor(true);
                        smasher[1].SetMotorSpeed(4);
                        smasher[1].EnableMotor(true);                        
                    }
                    else if (pb.Index == 1)
                    {
                        openDoor = true;
                        smasher[0].EnableMotor(false);
                        smasher[1].EnableMotor(false);
                    }
                }
            }
            else if (objB is DisplayObject)
            {
                if (((DisplayObject)objB).InstanceName == "elevatorBeam")
                {
                    needsTeleport = true;
                    if (stage != null)
                    {
                        stage.audio.PlaySound(Sfx.teleport);
                    }
                }
            }
        }

        public override void OnUpdateComplete(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            if(openDoor)
            {
                openDoor = false;
                trapDoor.OpenDoor();
            }

            if (needsTeleport)
            {
                needsTeleport = false;
                panicButton[0].IsDepressed = false;
                panicButton[1].IsDepressed = false;
                smasher[0].EnableMotor(false);
                smasher[1].EnableMotor(false);

                walker.body.SetLinearVelocity(Vector2.Zero);
                walker.body.Position = startPos;

                if (stage != null)
                {
                    stage.audio.StopSound(Sfx.metalRollLoop);
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
