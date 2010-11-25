using System;
using System.Collections.Generic;
using DDW.Display;
using DDW.Input;
using DDW.V2D;
using Sleepwalker.Panels;
using V2DRuntime.Display;
using V2DRuntime.Network;
using Sleepwalker.audio;
using Microsoft.Xna.Framework;
using V2DRuntime.V2D;

namespace Sleepwalker.Screens
{
    [ScreenAttribute(backgroundColor = 0xEEEEEE)]
    public class TitleScreen : Screen
    {
        private bool firstTimeDisplayed = false;// true;
        public Sprite bkg;

        public SplashPanel splashPanel;
        public MainMenuPanel mainMenuPanel;
        public ExitPanel exitPanel;
        public InstructionsPanel instructionsPanel;
        public ChooseLevelPanel chooseLevelPanel;
        public SignInPanel signInPanel;

        public Sprite buttonGuide;

        public Panel[] panels;
        private MenuState curState;
        private Stack<Panel> panelStack = new Stack<Panel>();

        public TitleScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public TitleScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        public override void Initialize()
        {
            base.Initialize();
            Stage.SpriteBatchMatrix = Microsoft.Xna.Framework.Matrix.Identity;
        }
        public override void Activate()
        {
            base.Activate();
            signInPanel.Unpause += new EventHandler(signInPanel_Unpause);
        }
        public override void Deactivate()
        {
            base.Deactivate();
            signInPanel.Unpause -= new EventHandler(signInPanel_Unpause);
        }

        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();

            panels = new Panel[] { splashPanel, mainMenuPanel, chooseLevelPanel, instructionsPanel, exitPanel, signInPanel };

            if (firstTimeDisplayed)
            {
                SetPanel(MenuState.Splash);
                splashPanel.Activate();
            }
            else
            {
                SetPanel(MenuState.MainMenu);
                mainMenuPanel.Activate();
            }

            firstTimeDisplayed = false;
        }

        void signInPanel_Unpause(object sender, EventArgs e)
        {
            SetPanel(MenuState.MainMenu);
        }
        public override void AddedToStage(EventArgs e)
        {
            base.AddedToStage(e);
        }
        public override void RemovedFromStage(EventArgs e)
        {
            SetPanel(MenuState.Empty);
            base.RemovedFromStage(e);
        }
        public override void SignInToLive()
        {
            SetPanel(MenuState.SignIn);
        }
        public override bool OnPlayerInput(int playerIndex, Move move, TimeSpan time)
        {
            if (move == Move.ButtonA)
            {
                SleepwalkerGame.activeController = (PlayerIndex)playerIndex;
            }

            if (curState != MenuState.MainMenu && move == Move.ButtonB)
            {
                SetPanel(MenuState.MainMenu);
                if (stage != null)
                {
                    stage.audio.PlaySound(Sfx.menuBack);
                }
            }
            else
            {
                panelStack.Peek().OnPlayerInput(playerIndex, move, time);
                if (stage != null)
                {
                    stage.audio.PlaySound(Sfx.menuForward);
                }
            }
            return false;
        }

        public void SetPanel(MenuState state)
        {
            switch (state)
            {
                case MenuState.Empty:
                    panelStack.Clear();
                    break;
                case MenuState.Splash:
                    panelStack.Push(splashPanel);
                    break;

                case MenuState.QuickGame:
                    OnStartGame();
                    break;

                case MenuState.ChooseLevel:
                    panelStack.Push(chooseLevelPanel);
                    break;

                case MenuState.Instructions:
                    panelStack.Push(instructionsPanel);
                    break;

                case MenuState.UnlockTrial:
                    SleepwalkerGame.instance.UnlockTrial();
                    break;

                case MenuState.MainMenu:
                    ((SleepwalkerGame)SleepwalkerGame.instance).StartMusic();
                    panelStack.Push(mainMenuPanel);
                    break;

                case MenuState.SignIn:
                    panelStack.Push(signInPanel);
                    break;

                case MenuState.Exit:
                    panelStack.Push(exitPanel);
                    //curPanel = exitPanel;
                    break;

            }

            Panel cp = panelStack.Count > 0 ? panelStack.Peek() : null;
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] == cp)
                {
                    if (!children.Contains(panels[i]))
                    {
                        AddChild(panels[i]);
                        panels[i].Activate();
                    }
                }
                else
                {
                    if (panels[i].IsOnStage)
                    {
                        panels[i].Deactivate();
                        RemoveChild(panels[i]);
                    }
                }
            }

            if (state == MenuState.MainMenu)
            {
                panelStack.Clear();
                panelStack.Push(mainMenuPanel);
            }

            curState = state;
        }
        protected void OnStartGame()
        {
            stage.NextScreen();
        }

        public MenuState nextState = MenuState.Empty;
        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);
            if (nextState != MenuState.Empty)
            {
                MenuState tempState = nextState;
                nextState = MenuState.Empty;
                SetPanel(tempState);
            }
        }
    }
    public enum MenuState
    {
        Empty,
        Splash,
        Begin,
        SignIn,
        MainMenu,
        NetworkGame,
        Lobby,
        HostGame,
        JoinGame,
        HighScores,
        Options,
        Instructions,
        UnlockTrial,
        QuickGame,
        ChooseLevel,
        Exit,
    }
}
