
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
    public class Tutorial4Screen : BaseScreen
    {
        public override string Title { get { return "Walking on Beams"; } }
        public override int Difficutly { get { return 2; } }
        
        public Swing[] swing;

        public Tutorial4Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Tutorial4Screen(SymbolImport si) : base(si)
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
        }
        public override void Activate()
        {
            base.Activate();
            walker.directionWalking = 1;
            targetZoom = 1.25f;
            followPlayer = false;
        }

    }
}
