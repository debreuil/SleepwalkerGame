using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DDW;

namespace Sleepwalker
{
    public class Camera
    {
        public Vector2 Translation;
        public float Zoom;
        public float Rotation;
        public float ShakeAmount = 1f;

        private Vector2 translationAdd;
        private float zoomAdd;
        private float rotationAdd;

        private Vector2 translationVelocity;
        private float zoomVelocity;
        private float rotationVelocity;

        public Vector2 screenSize;
        public Vector2 screenSizeCenter;
        private Vector3 screenSize3;
        private Vector3 screenSize3Center;
        private Vector2 screenTitleSafe;
        private Vector3 screenTitleSafe3;

        public Camera(Vector2 screenSize)
        {
            this.Translation = Vector2.Zero;
            this.Zoom = 1F;
            this.Rotation = 0F;

            this.translationAdd = Vector2.Zero;
            this.zoomAdd = 0;
            this.rotationAdd = 0;

            this.screenSize = screenSize;
            this.screenSizeCenter = screenSize / 2F;
            screenTitleSafe = screenSize * 1.25f;
            screenSize3 = new Vector3(screenSize, 0);
            screenSize3Center = screenSize3 / 2f;
            screenTitleSafe3 = screenSize3 * 1.25f;
        }
        public bool clampY = false;
        public void Update(float deltaTime)
        {
            //if (Translation.X - (screenSize.X / 2f * (1f / Zoom)) < 0)
            //{
            //    Translation.X = (screenSize.X / 2f * (1f / Zoom));
            //}

            // Spring for add values
            this.translationVelocity += (Vector2.Zero - translationAdd) * 0.7F;
            this.zoomVelocity += (0 - zoomAdd) * 0.2F;
            this.rotationVelocity += (0 - rotationAdd) * 0.4F;

            // Damping
            this.translationVelocity *= 0.89F;
            this.zoomVelocity *= 0.89F;
            this.rotationVelocity *= 0.89F;

            // Add deltas
            this.translationAdd += translationVelocity;
            this.zoomAdd += zoomVelocity;
            this.rotationAdd += rotationVelocity;

            // Set to zero
            translationAdd = (translationAdd.LengthSquared() < 0.01F) ? Vector2.Zero : translationAdd;
            //zoomAdd = (zoomAdd < 0.01F) ? 0 : zoomAdd;
            rotationAdd = (rotationAdd < 0.01F) ? 0 : rotationAdd;

            // Camera logic
            //Vector3 finalTranslation = new Vector3(Translation + translationAdd, 0f);
            float finalZoom = Zoom + zoomAdd;
            float finalRotation = Rotation + rotationAdd;

            float scalar = finalZoom * 0.8f; // titlesafe
            Vector2 extentSize = screenSize / scalar;

            Vector2 ratio = (Translation + translationAdd) / screenSize;
            Vector2 min = -(screenSize * 0.1f) / scalar;
            Vector2 max = screenSize - extentSize + (screenSize * 0.1f) / scalar;
            Vector2 pos = (max - min) * ratio + min;

            if (clampY)
            {
                if (Translation.Y < screenSizeCenter.Y * scalar)
                {
                    pos.Y = (Translation.Y + translationAdd.Y) - screenSizeCenter.Y;
                }
            }
            Vector2 posScaled = -pos * scalar;
            Vector3 finalTranslation2 = new Vector3(posScaled, 0);

            DDW.Display.Stage.SpriteBatchMatrix = 
                Matrix.CreateScale(scalar) * 
                Matrix.CreateTranslation(finalTranslation2);
        }

        private Random random = new Random();
        public void Shake(float intensity)
        {
            if (intensity < .1F)
            {
                translationAdd = Vector2.Zero;
                zoomAdd = 0;
                rotationAdd = 0;
            }
            else
            {
                intensity *= ShakeAmount;
                translationAdd += new Vector2((float)random.NextDouble(), (float)random.NextDouble()) * 10F * intensity;
                //zoomAdd += (float)random.NextDouble()* 0.01F;
                rotationAdd += (float)random.NextDouble() * 0.007F * intensity;
            }
        }
    }
}
