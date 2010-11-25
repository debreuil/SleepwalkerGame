using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Shaders;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;

namespace Sleepwalker
{
    public class BackgroundShader : V2DShader
    {
        public float level;
        public BackgroundShader(params float[] parameters)
            : base(parameters)
        {
            level = (parameters.Length > 0) ? parameters[0] : 1;
            LoadContent();
        }
        protected override void LoadContent()
        {
            effect = V2DGame.contentManager.Load<Effect>("BackgroundShader");
        }
        public override void Begin(SpriteBatch batch)
        {
            batch.End();
            effect.CurrentTechnique.Passes[0].Apply();
            batch.Begin(
             SpriteSortMode.Deferred,
             BlendState.NonPremultiplied,
             null, //SamplerState.AnisotropicClamp, 
             null, //DepthStencilState.None, 
             null, //RasterizerState.CullNone, 
             effect,
             Stage.SpriteBatchMatrix);
        }
        public override void End(SpriteBatch batch)
        {
            batch.End();
        }
        public override bool Equals(object obj)
        {
            bool result = base.Equals(obj);
            if (result && obj is BackgroundShader)
            {
                result = level.Equals(((BackgroundShader)obj).level);
            }
            return result;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + effect.GetHashCode() + (int)(level * 17);
        }
    }
}
