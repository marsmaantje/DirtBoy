using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;

namespace UIElements
{
    /// <summary>
    /// Indicator for how many lives the player has.
    /// It will indicate this by utilizing an array of animationSprites each representing one life
    /// </summary>
    class SoulCounter : Pivot
    {
        List<AnimationSprite> souls = new List<AnimationSprite>();
        string filename;
        float soulSize = 1;
        Vector2 boundsMin;
        Vector2 boundsMax;
        public int currentSouls
        {
            get => souls.Count;

            set => setHealth(value);
        }

        public SoulCounter(string filename, Vector2 min, Vector2 max, float pSoulSize) : base()
        {
            this.filename = filename;
            boundsMin = min;
            boundsMax = max;
            this.soulSize = pSoulSize;
            currentSouls = GlobalVariables.soulCounter;
            RecalculatePositions();
        }
        public void Update()
        {
            foreach (AnimationSprite soul in souls)
            {
                if (soul.currentFrame != soul.frameCount - 1)
                {
                    soul.Animate(18 * Time.deltaTime / 1000f);
                }
            }
            if (currentSouls != GlobalVariables.soulCounter)
            {
                setHealth(GlobalVariables.soulCounter);
            }
        }

        void setHealth(int targetSouls)
        {
            if (targetSouls != currentSouls && targetSouls >= 0)
            {
                int livesDifference = targetSouls - currentSouls;
                if (livesDifference > 0)
                {
                    for (int i = 0; i < livesDifference; i++)
                    {
                        AnimationSprite newHeart = new AnimationSprite(filename, 4, 2, 7, addCollider: false);
                        AddChild(newHeart);
                        souls.Add(newHeart);
                    }
                }
                RecalculatePositions();
            }
        }

        void RecalculatePositions()
        {
            Vector2 currentPosition = boundsMin;
            if (souls.Count > 0)
            {
                float spriteWidth = souls[0].width * soulSize;
                float spriteHeight = souls[0].height * soulSize;
                foreach (AnimationSprite heart in souls)
                {
                    heart.SetScaleXY(soulSize);
                    heart.SetXY(currentPosition.x, currentPosition.y);

                    currentPosition.x += spriteWidth;
                    if (currentPosition.x + spriteWidth > boundsMax.x)
                    {
                        currentPosition.x = boundsMin.x;
                        currentPosition.y += spriteHeight;
                    }
                }
            }
        }
    }
}
