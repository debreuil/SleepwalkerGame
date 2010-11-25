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
    public class Tutorial3Screen : BaseScreen
    {
        public override string Title { get { return "Using Trampolines"; } }
        public override int Difficutly { get { return 1; } }
        
        public Trampoline trampoline;

        public Tutorial3Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Tutorial3Screen(SymbolImport si) : base(si)
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
            targetZoom = 1.5f;
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
        }
    }
}
