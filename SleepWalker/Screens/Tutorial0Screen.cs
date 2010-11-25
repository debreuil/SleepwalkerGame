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
    public class Tutorial0Screen : BaseScreen
    {
        public override string Title { get { return "How to Move"; } }
        public override int Difficutly { get { return 1; } }
        
        public Tutorial0Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Tutorial0Screen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        public override void Initialize()
        {
            base.Initialize(); 
        }
        public override void Activate()
        {
            base.Activate();
            walker.directionWalking = 1;
            followPlayer = true;
            targetZoom = 2f;

            walker.directionWalking = -1;
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
        }

    }
}
