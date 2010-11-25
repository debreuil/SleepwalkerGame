using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Attributes;
using Box2D.XNA;
using Sleepwalker.audio;
using Microsoft.Xna.Framework.Audio;
using V2DRuntime.Audio;
using System;
using DDW.Display;
using V2DRuntime.Components;
using Sleepwalker.Screens;
using DDW.Input;

namespace Sleepwalker.Components
{
    public class LevelDescription : Sprite
    {
        //protected TextBox txTitle;
        protected Sprite levelText;
        protected Sprite levelThumbnails;
        protected Sprite[] difficultyIcon;
        protected Sprite[] levelIndicator;
        protected Sprite tutorialLevel;

        public int levelDescriptionCount;
        public bool levelSelected = false;
        public static int focusIndex = 0;

        public LevelDescription(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();

            levelSelected = false;
            levelDescriptionCount = (int)levelThumbnails.FrameCount;
            SetLevel(focusIndex);
        }
        public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
        {
            bool result = base.OnPlayerInput(playerIndex, move, time);

            if (result && isActive)// && !Guide.IsVisible)
            {
                if (move == Move.Left)
                {
                    PreviousFocus();
                }
                else if (move == Move.Right)
                {
                    NextFocus();
                }
                else if (focusIndex > -1 && move == Move.ButtonA)
                {
                    levelSelected = true; 
                }
                else if (move == Move.Empty)
                {
                }
            }
            return result;
        }
        public void NextFocus()
        {
            SetLevel(focusIndex + 1);
        }
        public void PreviousFocus()
        {
            SetLevel(focusIndex - 1);
        }
        public void SetLevel(int indx)
        {
            if (indx >= 0 && indx < levelThumbnails.FrameCount)
            {
                Screen scr = screen.Stage.GetScreenByIndex(indx + 1); // ignore title screen
                if (scr is BaseScreen)
                {
                    levelThumbnails.GotoAndStop((uint)indx);
                    levelText.GotoAndStop((uint)indx);
                    //txTitle.Text = ((BaseScreen)scr).Title;

                    int dif = ((BaseScreen)scr).Difficutly;
                    for (int i = 0; i < difficultyIcon.Length; i++)
                    {
                        difficultyIcon[i].Visible = i < dif ? true : false;
                    }

                    for (int i = 0; i < levelIndicator.Length; i++)
                    {
                        levelIndicator[i].GotoAndStop(i == indx ? 1u : 0u);
                    }

                    tutorialLevel.Visible = indx < 6;
                    
                    focusIndex = indx;
                }
                else
                {
                    throw new Exception("tried to get screen that isn't valid");
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
