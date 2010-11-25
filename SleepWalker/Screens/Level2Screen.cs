
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
using Sleepwalker.audio;

namespace Sleepwalker.Screens
{
    [V2DScreenAttribute(backgroundColor = 0x397093, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Level2Screen : BaseScreen
    {
        public override string Title { get { return "Walk this Way"; } }
        public override int Difficutly { get { return 2; } }
        
        public V2DSprite[] ropeBeam;
        public Teleporter teleporter;

        public MotorTracker[] motorTrackers;

        [PrismaticJointAttribute(enableMotor = true, maxMotorForce = 80000, motorSpeed = -8)]
        public PrismaticJoint[] pj;

        bool needsTeleport = false;

        public Level2Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level2Screen(SymbolImport si)  : base(si)
        {
            SymbolImport = si;
        }
        int[] speeds = new int[]{7, 8, 4, 10, 5};
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            motorTrackers = new MotorTracker[pj.Length];
            for (int i = 0; i < pj.Length; i++)
            {
                pj[i].SetMotorSpeed(speeds[i] * (i % 2 == 0 ? 1 : -1));

                if (stage != null)
                {
                    motorTrackers[i] = new MotorTracker(stage.audio, pj[i], Sfx.quietRollLoop);
                }
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
        }

        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);
            if (needsTeleport)
            {
                needsTeleport = false;
                walker.body.SetLinearVelocity(Vector2.Zero);
                walker.body.Position = teleporter.node[1].body.Position;
                if (stage != null)
                {
                    stage.audio.PlaySound(Sfx.teleport);
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < pj.Length; i++)
            {
                if (pj[i]._limitState == LimitState.AtUpper)
                {
                    pj[i].SetMotorSpeed(-speeds[i]);
                }
                else if (pj[i]._limitState == LimitState.AtLower)
                {
                    pj[i].SetMotorSpeed(speeds[i]);
                }

                motorTrackers[i].UpdateSound();
            }
        }
        public override void RemovedFromStage(EventArgs e)
        {
            if (motorTrackers != null)
            {
                for (int i = 0; i < motorTrackers.Length; i++)
                {
                    motorTrackers[i].StopAllSounds();
                    motorTrackers[i] = null;
                }
                motorTrackers = null;
            }
            base.RemovedFromStage(e);
        }
    }
}
