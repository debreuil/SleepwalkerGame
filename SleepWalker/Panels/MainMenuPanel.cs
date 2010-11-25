using System;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Sleepwalker.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;
using Microsoft.Xna.Framework.GamerServices;

namespace Sleepwalker.Panels
{
    public class MainMenuPanel : Panel
    {
        public ButtonTabGroup menuButtons;
        private MenuState[] buttonTargets = new MenuState[]
        {
            MenuState.QuickGame,
            MenuState.ChooseLevel,
            MenuState.Instructions,
            MenuState.UnlockTrial,
            MenuState.Exit,
        };
        public MainMenuPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
        {
            MenuState ms = buttonTargets[sender.Index];
            ((TitleScreen)parent).SetPanel(ms);
        }

        public override void Activate()
        {
            base.Activate();
            menuButtons.wrapAround = true;
            menuButtons.element[3].Visible = Guide.IsTrialMode;
            menuButtons.SetFocus(0);
            menuButtons.OnClick += new ButtonEventHandler(menuButtons_OnClick);
        }
        public override void Deactivate()
        {
            base.Deactivate();
            menuButtons.OnClick -= new ButtonEventHandler(menuButtons_OnClick);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            menuButtons.element[3].Visible = Guide.IsTrialMode;
        }
    }
}