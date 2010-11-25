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

namespace Sleepwalker.Screens
{

    [V2DScreenAttribute(backgroundColor = 0x397093, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Level1Screen : BaseScreen
    {
        public override string Title { get { return "Ticket to Ride"; } }
        public override int Difficutly { get { return 2; } }
        
        public Mover mover;
        public Swing swing;
        public ButtonGroup sideButtons;
        public Teleporter teleporter;

        int elevatorSpeed = 5;
        bool needsTeleport = false;

        public Level1Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level1Screen(SymbolImport si)  : base(si)
        {
            SymbolImport = si;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            sideButtons.SelectedIndex = 1;
        }
        public override void Activate()
        {
            base.Activate();
            followPlayer = false;
            targetZoom = 1.15f;
        }

        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is PressButton)
            {
                PressButton pb = (PressButton)objB;
                if (!pb.IsDepressed)
                {
                    mover.Speed = pb.Index == 0 ? elevatorSpeed : -elevatorSpeed;
                    sideButtons.SelectedIndex = pb.Index;
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
