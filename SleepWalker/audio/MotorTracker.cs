using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.XNA;
using V2DRuntime.Audio;
using Microsoft.Xna.Framework.Audio;

namespace Sleepwalker.audio
{
    public class MotorTracker
    {
        public PrismaticJoint pj;
        public string soundName;

        private bool isMotorPlaying = false;
        private Cue motorSound;
        private AudioManager audio;

        public MotorTracker(AudioManager audio, PrismaticJoint pj, string soundName)
        {
            this.audio = audio;
            this.pj = pj;
            this.soundName = soundName;
        }
        protected void SetMotorPlaying(bool soundOn)
        {
            if (soundOn)
            {
                if (motorSound == null)
                {
                    motorSound = audio.PlaySound(soundName);
                }
                else if (motorSound.IsPaused)
                {
                    motorSound.Resume();
                }
                float jScale = pj.GetJointSpeed() / 2f;
                jScale = jScale > 3 ? 3f : jScale < -6 ? -6f : jScale;
                motorSound.SetVariable("Pitch", jScale);
                motorSound.SetVariable("Volume", .8f);
            }
            else
            {
                if (motorSound != null && motorSound.IsPlaying)
                {
                    motorSound.Pause();
                }
            }
            isMotorPlaying = soundOn;
        }

        public void UpdateSound()
        {
            float jspeed = Math.Abs(pj.GetJointSpeed());
            if (!isMotorPlaying && jspeed > 0.5f)
            {
                SetMotorPlaying(true);
            }
            else if (isMotorPlaying && jspeed < 0.5f)
            {
                SetMotorPlaying(false);
            }
        }
        public void StopAllSounds()
        {
            if (motorSound != null)
            {
                motorSound.Stop(AudioStopOptions.Immediate);
            }
            motorSound = null;
        }
    }
}
