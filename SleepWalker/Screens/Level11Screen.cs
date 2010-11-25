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
using V2DRuntime.Enums;
using Sleepwalker.audio;
using Microsoft.Xna.Framework.Audio;

namespace Sleepwalker.Screens
{
    [V2DScreenAttribute(backgroundColor = 0x000001, gravityX = 0, gravityY = 60, debugDraw = false)]
    public class Level11Screen : BaseScreen
    {
        public override string Title { get { return titles[songIndex]; } }
        public override int Difficutly { get { return difficulties[songIndex]; } }

        [V2DSpriteAttribute(restitution = 3f)]
        public Trampoline[] trampoline;

        public PianoKey[] pianoKey;
        public V2DSprite[] pianoHint;

        [RevoluteJointAttribute(motorSpeed=0, maxMotorTorque = 100, enableMotor = true)]
        public RevoluteJoint rOpenBed;

        private bool isDoorOpen = false;


        private int songIndex;
        private static int[][] songs = 
        {
            new int[]{0,2,4,2,0},
            new int[]{0,0,4,4,5,5,4}, // twinkle
            new int[]{0,0,0,1,2,2,1,2,3,4},// row row row your boat
        };
        private static int[][] timings = 
        {
            new int[]{4,4,4,4,4,4},
            new int[]{4,4,4,4,4,4,4,4}, // twinkle
            new int[]{6,6,6,4,2,8,4,2,4,2,8},// row row row your boat
        };
        private static string[] titles = { "Prelude to an Ode", "Ode", "Ode to a Prelude" };
        private static int[] difficulties = { 2,3,4 };

        private Stack<int> noteStack = new Stack<int>();
        private int noteWait = 0;
        private int noteDelay = 300;
        private int playingNoteIndex = -1;

        public Level11Screen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public Level11Screen(SymbolImport si, int songIndex) : base(si)
        {
            this.songIndex = songIndex;
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            for (int i = 0; i < trampoline.Length; i++)
            {
                trampoline[i].tramp.body.GetFixtureList().SetRestitution(3f);                
            }
        }

        public override void Activate()
        {
            base.Activate();
            followPlayer = true;
            targetZoom = 1.1f;
            ClearBoundsBody(EdgeName.Top);

            for (int i = 0; i < pianoHint.Length; i++)
            {
                pianoHint[i].Visible = false;
            }

            isDoorOpen = false;
            noteStack.Clear();

            if (stage != null)
            {
                stage.audio.PlaySound(Sfx.crowdTalk);
            }

            PlaySequence();
        }
        public void EndSequence()
        {
            for (int i = 0; i < pianoHint.Length; i++)
            {
                pianoHint[i].Visible = false;
            }
            playingNoteIndex = -1;
            SetHint();
        }
        public void PlaySequence()
        {
            playingNoteIndex = 0;
        }
        public void NextNote()
        {
            if (songs[songIndex].Length > playingNoteIndex && playingNoteIndex != -1)
            {
                int key = songs[songIndex][playingNoteIndex++];
                pianoKey[key].PlayNote();
            }
            else
            {
                EndSequence();
            } 
            SetHint();
        }
        private void SetHint()
        {
            if (playingNoteIndex == -1 && noteStack.Count < songs[songIndex].Length)
            {
                int key = songs[songIndex][noteStack.Count];
                for (int i = 0; i < pianoHint.Length; i++)
                {
                    pianoHint[i].Visible = (i == key) ? true : false;
                }
            }
            else
            {
                for (int i = 0; i < pianoHint.Length; i++)
                {
                    pianoHint[i].Visible = false;
                }
            }
        }

        protected override void OnWalkerContact(object player, object objB, Fixture fixtureA, Fixture fixtureB)
        {
            base.OnWalkerContact(player, objB, fixtureA, fixtureB);

            if (objB is V2DSprite)
            {
                if (((V2DSprite)objB).Parent is Trampoline)
                {
                    EndSequence();

                    Walker p = (Walker)player;
                    if (p.Visible)
                    {
                        float ls = p.body.GetLinearVelocity().LengthSquared();

                        if (ls > 400f)
                        {
                            ((Trampoline)((V2DSprite)objB).Parent).Bounce();
                        }
                        else if (ls > 200f)
                        {
                            ((Trampoline)((V2DSprite)objB).Parent).BounceSmall();
                        }

                        if (ls > 1000f)
                        {
                            p.body.SetLinearVelocity(p.body.GetLinearVelocity() * .8f);
                        }
                    }
                    int index = ((V2DSprite)objB).Parent.Index;
                    pianoKey[index].PlayNote();

                    noteStack.Push(index);
                    CheckForWin();
                    SetHint();
                }
            }
        }

        private bool CheckForWin()
        {
            bool result = false;
            if (!isDoorOpen)
            {
                result = true;
                int[] playedNotes = noteStack.ToArray();
                int minLen = Math.Min(noteStack.Count, songs[songIndex].Length);
                for (int i = 0; i < minLen; i++)
                {
                    if (playedNotes[playedNotes.Length - 1 - i] != songs[songIndex][i])
                    {
                        result = false;
                        noteStack.Clear();

                        if (stage != null)
                        {
                            if(!stage.audio.IsPlaying(Sfx.crowdOh))
                            {
                                stage.audio.PlaySound(Sfx.crowdOh, OnRestartReady);
                            }
                        }
                        else
                        {
                            ResetLevel();
                        }
                    }
                }

                if (result == true && noteStack.Count >= songs[songIndex].Length)
                {
                    if (stage != null)
                    {
                        stage.audio.PlaySound(Sfx.crowdApplause);
                    }
                    isDoorOpen = true;
                }
            }
            return result;
        }

        private void OnRestartReady(Cue c)
        {
            ResetLevel();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (playingNoteIndex != -1)
            {
                noteWait += gameTime.ElapsedGameTime.Milliseconds;
                if (noteWait > noteDelay * (timings[songIndex][playingNoteIndex] / 4f))
                {
                    noteWait = 0;//-= (int)(noteDelay * (timings[songIndex][playingNoteIndex] / 4f));
                    NextNote();
                }
            }
        }
        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);
            if (isDoorOpen && rOpenBed != null)
            {
                world.DestroyJoint(rOpenBed);
                rOpenBed = null;
            }
        }
    }
}
