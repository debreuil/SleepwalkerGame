using System;
using System.Collections.Generic;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DDW.Display;
using V2DRuntime.Components;
using Box2D.XNA;
using V2DRuntime.Attributes;
using Sleepwalker.Screens;
using Sleepwalker.audio;
using Microsoft.Xna.Framework.Audio;

namespace Sleepwalker.Components
{
    public enum AnimState
    {
        Empty,
        Walk,
        Turn,
        Rise,
        Fall,
        Land,
        Sleep,
    }
    public class Walker : V2DSprite
    {
        private Cue walkSound;
        private bool isWalkPlaying = false;
        private bool isReleased = true;

        float timeSinceGroundHit = 0;
        int contactCount;
        //bool applyJump = false;

        public int directionWalking = -1;
        float delay = 0;
        float jumpDelay = 0;

        public Walker(Texture2D texture, V2DInstance instance)  : base(texture, instance)
        {

        }
        
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Activate()
        {
            base.Activate();
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            isReleased = GamePad.GetState(SleepwalkerGame.activeController).IsButtonUp(Buttons.A);
            this.PlayAll();
            this.Stop();
        }
        public void BeginContact()
        {
            contactCount++;

            if (animState != AnimState.Fall && animState != AnimState.Land && stage != null)
            {
                stage.audio.PlaySound(Sfx.woodHit);
            }
        }

        public void EndContact()
        {
            contactCount--;
        }

        //public void OnCollision()//Vector2 normal)
        //{
        //    //if (Math.Abs(normal.Y) > 0.5F)
        //    //{
        //        timeSinceGroundHit = 0;
        //        applyJump = true;
        //    //}
        //}
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            delay += deltaTime;
            jumpDelay += deltaTime;

            //bool touchingGround = (contactCount > 0)  ? true : false;

            // Ground hit
            timeSinceGroundHit += deltaTime;

            //if ( applyJump)
            //{
            //    applyJump = false;
            //    this.body.ApplyImpulse(new Vector2(0, -200), this.body.GetWorldCenter());
            //}
            //if (!touchingGround)
            //{
            //    this.body.SetLinearVelocity(this.body.GetLinearVelocity() * 0.95F);
            //}
            this.body.SetLinearVelocity(this.body.GetLinearVelocity() * 0.98F);

            // Walking
            PlayerIndex plIndex = SleepwalkerGame.activeController;
            bool isPressed = GamePad.GetState(plIndex).IsButtonDown(Buttons.A);// || GamePad.GetState(plIndex).IsButtonDown(Buttons.B);

            if (isPressed && isReleased)
            {
                ((BaseScreen)screen).hasPressedA = true;
                isReleased = false;
                directionWalking = -directionWalking;
                directionChanged = true;
                if (contactCount > 0 && delay > .4f)
                {
                    this.body.ApplyLinearImpulse(new Vector2(0f, -200f), this.body.GetWorldCenter());
                }
                delay = 0;
            }

            if (!isReleased)
            {
                isReleased = GamePad.GetState(plIndex).IsButtonUp(Buttons.A);
            }

            SetWalkFrame();

            //if (touchingGround)
            //{
            //    Console.WriteLine(this.body.GetLinearVelocity().Length());
            //    this.body.ApplyImpulse(new Vector2(12 * directionWalking, 0), this.body.GetWorldCenter());
            //}

            //if (touchingGround)// && this.body.GetLinearVelocity().Length() > 20F)
            //{
            //    float sign = Math.Sign(20F - this.body.GetLinearVelocity().Length());

                this.body.ApplyLinearImpulse(new Vector2(12 * directionWalking, 0), this.body.GetWorldCenter());
            //}

            //// Jumping
            //if (gamePad.IsButtonDown(Buttons.A) && jumpDelay > 0.1F)
            //{
            //    jumpDelay = 0;

            //    this.body.ApplyImpulse(new Vector2(0, -80), this.body.GetWorldCenter());
            //}

            base.Update(gameTime);
        }



        readonly uint walkRight = 0;
        readonly uint walkLeft = 3;
        readonly uint fallRight = 7;
        readonly uint fallLeft = 8;
        readonly uint landRight = 10;
        readonly uint landLeft = 15;
        readonly uint risingRight = 20;
        readonly uint risingLeft = 21;
        readonly uint turnRight = 29;
        readonly uint turnLeft = 49;

        readonly float fallThreshold = 18f;
        readonly float stillThreshold = 2.0f; //1.5f;
        readonly uint landFrames = 3;

        AnimState animState = AnimState.Empty;
        private bool directionChanged = true;
        private bool isTurning = false;

        private void SetWalkFrame()
        {
            Vector2 sp = body.GetLinearVelocity();
            float xSpeed = sp.X;
            float ySpeed = sp.Y;

            bool isFalling = ySpeed > fallThreshold;
            bool isRising = ySpeed <= -fallThreshold;
            bool isStillOnX = (xSpeed < stillThreshold) && (xSpeed > -stillThreshold);
            bool isStillOnY = (ySpeed < stillThreshold) && (ySpeed > -stillThreshold); //(isFalling || isRising) && (ySpeed < stillThreshold) && (ySpeed > -stillThreshold);

            bool resting = 
                ((CurChildFrame >= turnLeft + 2) && (CurChildFrame < FrameCount)) ||
                ((CurChildFrame >= turnRight + 2) && (CurChildFrame < turnLeft));

            AnimState prevState = animState;

            // turn
            if (directionChanged)
            {
                directionChanged = false;
                isTurning = true;
                animState = AnimState.Turn;
                this.GotoAndPlay(directionWalking > 0 ? turnLeft : turnRight); //turnAndPause);
                if (stage != null)
                {
                    stage.audio.PlaySound(Sfx.turn);
                }
            }
            // turning end
            else if (isTurning && resting)
            {
                isTurning = false;
                animState = AnimState.Empty;
                this.GotoAndStop(directionWalking > 0 ? walkRight : walkLeft);
            }
            else if (animState == AnimState.Land && ((CurChildFrame == landRight + landFrames) || (CurChildFrame == landLeft + landFrames)))
            {
                animState = AnimState.Empty;
                this.GotoAndStop(directionWalking > 0 ? turnRight : turnLeft); // just in case a fall through below
                if (stage != null)
                {
                    stage.audio.PlaySound(Sfx.grunt);
                }
            }
            else if ((animState != AnimState.Fall) && contactCount == 1) // jump up to ledge and land on it
            {
                animState = AnimState.Empty;
            }


            // land
            if ((animState == AnimState.Fall && (isStillOnY || contactCount > 0)))
            {
                this.GotoAndPlay(directionWalking > 0 ? landRight : landLeft);
                animState = AnimState.Land;
                if (stage != null)
                {
                    stage.audio.PlaySound(Sfx.bodyLand);
                }
            }
            // fall
            else if ((animState != AnimState.Fall) && isFalling && contactCount == 0)
            {
                this.GotoAndStop(directionWalking > 0 ? fallRight : fallLeft);
                animState = AnimState.Fall;
            }
            // rise
            else if ((animState != AnimState.Rise) && isRising)
            {
                this.GotoAndStop(directionWalking > 0 ? risingRight : risingLeft);
                animState = AnimState.Rise;
            }
            // walk
            else if ((animState != AnimState.Walk && animState != AnimState.Land && animState != AnimState.Fall) &&
                    !isStillOnX && !isTurning && contactCount > 0)
            {
                this.GotoAndStop(directionWalking > 0 ? walkRight : walkLeft);
                animState = AnimState.Walk;
            }
            // sleep
            else if ( (animState != AnimState.Sleep) && 
                (CurChildFrame == (turnLeft - 1) || CurChildFrame == (FrameCount - 1)) )
            {
                this.Stop();
                animState = AnimState.Sleep;
            }
            // hit wall and pause
            else if (isStillOnX && !resting && (contactCount > 1) && CurChildFrame < turnRight)
            {
                this.GotoAndPlay(directionWalking > 0 ? turnRight : turnLeft); //turnAndPause);
                animState = AnimState.Turn;
            }
            else if (animState == AnimState.Empty)
            {
                this.GotoAndStop(directionWalking > 0 ? walkRight : walkLeft);
                isTurning = false;
                animState = AnimState.Walk;
            }

            // sound
            if (prevState == AnimState.Walk && animState != AnimState.Walk)
            {
                SetWalkPlaying(false);
            }
            else if (prevState != AnimState.Walk && animState == AnimState.Walk)
            {
                SetWalkPlaying(true);
            }
        }

        protected void SetWalkPlaying(bool soundOn)
        {
            if (soundOn)
            {
                if (walkSound == null)
                {
                    if (stage != null)
                    {
                        walkSound = stage.audio.PlaySound(Sfx.walkCycle);
                    }
                }
                else if (walkSound.IsPaused)
                {
                    walkSound.Resume();
                }
            }
            else
            {
                if (walkSound != null && walkSound.IsPlaying)
                {
                    walkSound.Pause();
                }
            }
            isWalkPlaying = soundOn;
        }
        public void StopAllSounds()
        {
            if (walkSound != null)
            {
                walkSound.Stop(AudioStopOptions.Immediate);
            }
        }
        public override void RemovedFromStage(EventArgs e)
        {
            StopAllSounds();
            walkSound = null;
            base.RemovedFromStage(e);
        }
    }
}
