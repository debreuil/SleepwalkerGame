using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Shaders;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;
using Microsoft.Xna.Framework;

namespace Sleepwalker
{
    public class ObjectShader : V2DShader
    {
        public float level;
        public ObjectShader(params float[] parameters)
            : base(parameters)
        {
            level = (parameters.Length > 0) ? parameters[0] : 1;
            LoadContent();
        }
        protected override void LoadContent()
        {
            effect = V2DGame.contentManager.Load<Effect>("ObjectShader");
            lightingEffect = V2DGame.contentManager.Load<Effect>("LightingEffect");

            //spriteBatch = new SpriteBatch(V2DGame.instance.GraphicsDevice);

            screenResolve = new RenderTarget2D(V2DGame.instance.GraphicsDevice, 1280, 720);
        }

        //SpriteBatch spriteBatch;
        Effect lightingEffect;
        RenderTarget2D screenResolve;

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
            //run post lighting effect here because all objects have been drawn

            batch.End();
            //V2DGame.instance.spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            //V2DGame.instance.GraphicsDevice.ResolveBackBuffer(screenResolve);
 //           batch.GraphicsDevice.SetRenderTarget(screenResolve);
            effect.CurrentTechnique.Passes[0].Apply();
            batch.Begin(
             SpriteSortMode.Deferred,
             BlendState.NonPremultiplied,
             null, //SamplerState.AnisotropicClamp, 
             null, //DepthStencilState.None, 
             null, //RasterizerState.CullNone, 
             effect,
             Stage.SpriteBatchMatrix);

            batch.Draw
                (
                    screenResolve,
                    new Microsoft.Xna.Framework.Rectangle
                        (
                            0,
                            0,
                            V2DGame.instance.GraphicsDevice.PresentationParameters.BackBufferWidth,
                            V2DGame.instance.GraphicsDevice.PresentationParameters.BackBufferHeight
                        ), Color.White
                );
 //           batch.GraphicsDevice.SetRenderTarget(null);

            //lightingEffect.CurrentTechnique.Passes[0].End();
            //lightingEffect.End();
        }
        public override bool Equals(object obj)
        {
            bool result = base.Equals(obj);
            if (result && obj is ObjectShader)
            {
                result = level.Equals(((ObjectShader)obj).level);
            }
            return result;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + effect.GetHashCode() + (int)(level * 17);
        }
    }
}
