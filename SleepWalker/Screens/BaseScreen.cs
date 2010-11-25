using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.Display;
using DDW.V2D;
using Sleepwalker.Components;
using V2DRuntime.Attributes;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using V2DRuntime.Shaders;
using Box2D.XNA;
using Sleepwalker.audio;
using Microsoft.Xna.Framework.Audio;
using Sleepwalker.Panels;
using DDW.Input;

namespace Sleepwalker.Screens
{
    //public delegate void ContactDelegate(object objA, object objB);

    public abstract class BaseScreen : V2DScreen
    {
        //public static Dictionary<Type, ContactDelegate> contactTypes = new Dictionary<Type, ContactDelegate>();

        public GameOverlay gameOverlay;

        [V2DShaderAttribute(shaderType = typeof(BackgroundShader))]
        [V2DSpriteAttribute(depthGroup = -1)]
        public Sprite bkg;

        [V2DSpriteAttribute(isStatic = true, friction=.1f)]
        public V2DSprite[] beam;

        [V2DSpriteAttribute(isStatic = true, isSensor = true)]
        public WinTarget target;

        [V2DShaderAttribute(shaderType = typeof(ObjectShader))]
        [V2DSpriteAttribute(restitution = .1f, friction = .1f, fixedRotation = true)]
        public Walker walker;

        public Sprite targetAnim;

        public bool restartLevel = false;
        public bool skipLevel = false;
        public bool exitToMainMenu = false;
        protected bool levelOver = false;
        private int endDelay;

        public Camera Camera;
        public bool followPlayer = false;
        protected float targetZoom = 1.25f;

        public abstract string Title { get; }
        public abstract int Difficutly { get; }
        

        public BaseScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public BaseScreen(SymbolImport si)  : base(si)
        {
        }

        public override void Initialize()
        {
            Camera = new Camera(screen.ClientSize);
            base.Initialize();
        }
        public override void Activate()
        {
            base.Activate();

            restartLevel = false;
            levelOver = false;
            skipLevel = false;
            exitToMainMenu = false;

            target.PlayAll();
            target.GotoAndStop(0);
            targetAnim.GotoAndStop(0);
            walker.Visible = true;
            walker.directionWalking = -1;

            Camera.clampY = false;
            followPlayer = true;
            Camera.Zoom = 1f;

            cameraStep = 0;
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            
            hasPressedA = false;
            hasZoomedIn = false;
            contactTypes.Add(walker.GetType(), OnWalkerContact);
        }
        //public override void BeginContact(Contact contact)
        //{
        //    base.BeginContact(contact);

        //    object objA = contact.GetFixtureA().GetBody().GetUserData();
        //    object objB = contact.GetFixtureB().GetBody().GetUserData();

        //    foreach (Type t in contactTypes.Keys)
        //    {
        //        if (objA.GetType() == t)
        //        {
        //            contactTypes[t](objA, objB);
        //            break;
        //        }
        //        else if (objB.GetType() == t)
        //        {
        //            contactTypes[t](objB, objA);
        //            break;
        //        }
        //    }
        //}
        public override void EndContact(Contact contact)
        {
            base.EndContact(contact);
            object objA = contact.GetFixtureA().GetBody().GetUserData();
            object objB = contact.GetFixtureB().GetBody().GetUserData();

            if (objA is Walker || objB is Walker)
            {
                walker.EndContact();
            }
        }
        public override void PostSolve(Contact contact, ref Box2D.XNA.ContactImpulse impulse)
        {
            float intensity = impulse.normalImpulses[0] / 2800F;
            //intensity = Math.Min(1F, intensity);
            if (intensity > 0.20F)
            {
                Camera.Shake(intensity);
                OnShake(contact, intensity);
            }
            base.PostSolve(contact, ref impulse);
        }

        protected virtual void OnShake(Contact contact, float intensity)
        {
            if (!(contact.GetFixtureA().GetBody().GetUserData() is Walker) && !(contact.GetFixtureB().GetBody().GetUserData() is Walker))
            {
                if (stage != null)
                {
                    if (intensity < .5f)
                    {
                        Cue c = stage.audio.PlaySound(Sfx.woodHit);
                        c.SetVariable("Volume", 1);
                    }
                    else
                    {
                        float vol = intensity / 4 + .75f;
                        vol = vol > 1 ? 1f : vol < 0.75f ? 0.75f : vol;

                        Cue c = stage.audio.PlaySound(Sfx.smash);
                        c.SetVariable("Volume", vol);
                    }
                }
            }
        }
        public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, System.TimeSpan time)
        {
            bool result = base.OnPlayerInput(playerIndex, move, time);

            if (!gameOverlay.hasActivePanel)
            {
                if (result && move.Releases == Buttons.X)
                {
                    ResetLevel();
                }
                if (result && move == Move.Start)
                {
                    SleepwalkerGame.activeController = (PlayerIndex)playerIndex;
                    gameOverlay.PauseGame();
                }
                //else if (result && move.Releases == Buttons.LeftShoulder)
                //{
                //    SkipLevel();
                //}
            }
            //else if (result && levelOver && move.Releases == Buttons.A && endDelay <= 0)
            //{
            //    levelOver = false;
            //    DestroyView();
            //    stage.NextScreen();
            //}

            return false;
        }
        protected void ResetLevel()
        {
            restartLevel = true;
        }
        protected void SkipLevel()
        {
            skipLevel = true;
        }
        public void ExitToMainMenu()
        {
            exitToMainMenu = true;
        }
        public override void SignInToLive()
        {
            gameOverlay.SignInToLive();
        }
        
        public override void OnUpdateComplete(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);

            if (skipLevel)
            {
                skipLevel = false;
                stage.NextScreen();
            }
            else if (restartLevel)
            {
                restartLevel = false;
                stage.SetScreen(this.instanceName);
            }
            else if (exitToMainMenu)
            {
                exitToMainMenu = false;
                stage.SetScreen("titleScreens");
            }
            else if (levelOver)
            {
                gameOverlay.EndRound();
                //if (!gameOverlay.Visible)
                //{
                //    followPlayer = false;
                //    Camera.Zoom = 1f;
                //    Camera.ShakeAmount = 0f;
                //    endDelay = 1000;
                //    gameOverlay.Visible = true;
                //}
                //else
                //{
                //    levelOver = false;
                //    DestroyView();
                //    stage.NextScreen();
                //}
            }
        }
        protected virtual void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            walker.BeginContact();

            if (objB is WinTarget)
            {
                if (targetAnim != null)
                {
                    walker.StopAllSounds();
                    walker.DestroyView();
                    DestroyAfterUpdate(walker);

                    if (stage != null)
                    {
                        stage.audio.PlaySound(Sfx.snore);
                    }
                    targetAnim.GotoAndPlay(1);
                    targetAnim.PlayheadWrap += new V2DRuntime.Display.AnimationEvent(win_PlayheadWrap);
                }
            }
            if (stage != null)
            {
                Cue c = stage.audio.PlaySound(Sfx.smallHit);
                c.SetVariable("Volume", .6f);
            }
        }

        void win_PlayheadWrap(DisplayObjectContainer sender)
        {
            targetAnim.PlayheadWrap -= new V2DRuntime.Display.AnimationEvent(win_PlayheadWrap);
            levelOver = true;
            targetAnim.GotoAndStop(targetAnim.FrameCount - 1);
        }

        public override void Removed(EventArgs e)
        {
            base.Removed(e);
            if(targetAnim != null)
            {
                targetAnim.PlayheadWrap -= new V2DRuntime.Display.AnimationEvent(win_PlayheadWrap);
            }
            //contactTypes.Clear();
        }

        public bool hasPressedA = false;
        private bool hasZoomedIn = false;
        private int cameraStep = 0;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (gameOverlay.hasActivePanel)
            {
                base.ManageInput(gameTime);

                //if (Camera.Zoom != 1f)
                //{
                //    Camera.Zoom = 1f;
                //    Camera.Translation = Camera.screenSizeCenter;
                //    Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                //}
                gameOverlay.Update(gameTime);
            }
            else
            {
                base.Update(gameTime);

                // Camera logic
                if (!hasPressedA)
                {
                    Camera.Zoom = 1f;
                    Camera.Translation = Camera.screenSizeCenter;
                }
                else if (!hasZoomedIn && hasPressedA)
                {
                    if (cameraStep++ < 100)
                    {
                        Camera.Zoom = (targetZoom - 1f) / 100f * cameraStep + 1f;
                        if (followPlayer)
                        {
                            Camera.Translation = (walker.Position - Camera.screenSizeCenter) / 100 * cameraStep + Camera.screenSizeCenter;
                        }
                        else
                        {
                            Camera.Translation = Camera.screenSizeCenter;
                        }
                    }
                    else
                    {
                        cameraStep = 0;
                        hasZoomedIn = true;
                    }
                }
                else if (followPlayer)
                {
                    Camera.Translation = walker.Position;// *Camera.Zoom;
                }
                else
                {
                    Camera.Translation = Camera.screenSizeCenter;
                }

                //Camera.Zoom = 1.5F;
                Camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                // Keys
                KeyboardState ks = Keyboard.GetState();
                if (ks.IsKeyDown(Keys.Q))
                {
                    Camera.Shake(1);
                }

                if (levelOver && endDelay > 0)
                {
                    endDelay -= gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            if (stage != null)
            {
                stage.audio.Update();
            }
        }
    }
}
