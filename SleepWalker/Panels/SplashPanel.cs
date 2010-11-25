using System;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Sleepwalker.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using DDW.Display;

namespace Sleepwalker.Panels
{
	public class SplashPanel : Panel
	{
        public Sprite videoHolder;

        Video logoVideo;
        VideoPlayer videoPlayer;
        bool started = false;

		public SplashPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }


        public override void Activate()
        {
            base.Activate();
            videoPlayer = new VideoPlayer();
            logoVideo = V2DGame.instance.Content.Load<Video>(@"DDWLogoMovie");

            videoPlayer.Play(logoVideo);
            started = true;
        }
        public override void Deactivate()
        {
            base.Deactivate();

            if (videoPlayer != null)
            {
                videoPlayer.Stop();
                videoPlayer.Dispose();
            }
            logoVideo = null;
            videoPlayer = null;
        }
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);

            if (videoPlayer.State == MediaState.Playing)
            {
                //batch.Draw(videoPlayer.GetTexture(), new Rectangle((int)videoHolder.X, (int)videoHolder.Y, logoVideo.Width, logoVideo.Height), Color.White);
                batch.Draw(videoPlayer.GetTexture(), new Vector2((int)videoHolder.X, (int)videoHolder.Y), new Rectangle(1,1, logoVideo.Width - 2, logoVideo.Height - 2), Color.White);//new Color(0xEE, 0xEE, 0xEE)
            }
            else if (started)
            {
                started = false;
                ((TitleScreen)parent).nextState = MenuState.MainMenu;
            }
        }
	}
}
