using GXPEngine;
using GXPEngine.Core;
using System;

namespace Scripts
{
    public class SFX : GameObject
    {
        Sound landingSFX, fallDamageSFX;

        public float musicVolume = 5;
        public float effectVolume = 5;
        
        public Vector2 musicVolumePoz = new Vector2(260, 77.5f);
        public Vector2 effectVolumePoz = new Vector2(260, 169.5f);
        public SFX()
        {
            landingSFX = new Sound("OST/Landing.mp3"); //Load sound that plays when you land on the ground
            fallDamageSFX = new Sound("OST/fallDamage.wav"); //Load sound that plays when you take fallDamage
        }

        public void changeVolume(string settingName = "", int increasingValue = 0)
        {
            if (settingName == "Effect")
            {
                effectVolume = Mathf.Clamp(effectVolume += increasingValue, 0, 10);
            }
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
