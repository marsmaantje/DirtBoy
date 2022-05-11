using GXPEngine.Core;
using System;

namespace GXPEngine
{
    public class SFX : GameObject
    {
        SoundChannel backgroundMusic;
        Sound landingSFX, fallDamageSFX;

        public float musicVolume = 5;
        public float effectVolume = 5;
        
        public Vector2 musicVolumePoz = new Vector2(260, 77.5f);
        public Vector2 effectVolumePoz = new Vector2(260, 169.5f);
        public SFX()
        {
            backgroundMusic = new Sound("The_Endless_Journey.ogg", true).Play(); //Play the backgroundMusic (that loops)
            Console.WriteLine("");
            landingSFX = new Sound("Landing.mp3"); //Load sound that plays when you land on the ground
            fallDamageSFX = new Sound("fallDamage.wav"); //Load sound that plays when you take fallDamage
            backgroundMusic.IsPaused = false;
            this.SetVolume();
        }

        public void changeVolume(string settingName = "", int increasingValue = 0)
        {
            if (settingName == "Music")
            {
                musicVolume = Mathf.Clamp(musicVolume += increasingValue, 0, 10);
            }
            else if (settingName == "Effect")
            {
                effectVolume = Mathf.Clamp(effectVolume += increasingValue, 0, 10);
            }
            SetVolume();
        }

        void SetVolume()
        {
            backgroundMusic.Volume = musicVolume * 0.1f;

        }

        public void PlayLanding()
        {
            landingSFX.Play(false, 0, effectVolume * 0.1f, 0);
        }
        public void PlayFallDamage()
        {
            fallDamageSFX.Play(false, 0, effectVolume * 0.1f, 0);
        }
    }
}
