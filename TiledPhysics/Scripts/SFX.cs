using GXPEngine;
using GXPEngine.Core;
using System;
using TiledMapParser;

namespace Scripts
{
    public class SFX : GameObject
    {
        Sound soundEffect;

        public float effectVolume = 0.5f;
        
        public Vector2 effectVolumePoz = new Vector2(260, 169.5f);
        public SFX(string soundEffect)
        {
            this.soundEffect = new Sound(soundEffect);
        }

        public void changeVolume(float increasingValue = 0f)
        {
            effectVolume = Mathf.Clamp(effectVolume += increasingValue, 0, 1);
        }

        public void Play()
        {
            soundEffect.Play(false, 0, effectVolume * 0.1f, 0);
        }
    }
}
