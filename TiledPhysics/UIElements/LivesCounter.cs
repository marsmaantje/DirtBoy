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
    class LivesCounter : Pivot
    {
        List<AnimationSprite> hearts = new List<AnimationSprite>();
        List<AnimationSprite> disappearingHearts = new List<AnimationSprite>();
        string filename;
        float heartSize = 1;
        Vector2 boundsMin;
        Vector2 boundsMax;

        public int currentLives
        {
            get => hearts.Count;

            set => setHealth(value);
        }

        public LivesCounter(string filename, Vector2 min, Vector2 max, float heartSize, int currentHealth) : base()
        {
            this.filename = filename;
            boundsMin = min;
            boundsMax = max;
            this.heartSize = heartSize;
            currentLives = currentHealth;
            RecalculatePositions();
        }

        public void Update()
        {
            foreach (AnimationSprite heart in hearts)
            {
                if (heart.currentFrame != heart.frameCount - 1)
                {
                    heart.Animate(18 * Time.deltaTime / 1000f);
                }
            }

            List<AnimationSprite> removedHearts = new List<AnimationSprite>();
            foreach (AnimationSprite heart in disappearingHearts)
            {
                if (heart.currentFrame == 0)
                {
                    heart.LateDestroy();
                    removedHearts.Add(heart);
                }

                heart.SetCycle(heart.currentFrame - 1, 2);
                heart.Animate(18 * Time.deltaTime / 1000f);
            }

            foreach (AnimationSprite heart in removedHearts)
            {
                disappearingHearts.Remove(heart);
            }
        }

        void setHealth(int targetHealth)
        {
            if (targetHealth != currentLives && targetHealth >= 0)
            {
                int livesDifference = targetHealth - currentLives;
                if (livesDifference > 0)
                {
                    for (int i = 0; i < livesDifference; i++)
                    {
                        AnimationSprite newHeart = new AnimationSprite(filename, 4, 2, 7, addCollider: false);
                        AddChild(newHeart);
                        hearts.Add(newHeart);
                    }
                }
                else
                {
                    for (int i = 0; i < -livesDifference; i++)
                    {
                        AnimationSprite removedHeart = hearts[0];
                        disappearingHearts.Insert(0, removedHeart);
                        hearts.Remove(removedHeart);
                    }
                }
                RecalculatePositions();
            }
        }

        void RecalculatePositions()
        {
            Vector2 currentPosition = boundsMin;
            if (hearts.Count > 0)
            {
                float spriteWidth = hearts[0].width * heartSize;
                float spriteHeight = hearts[0].height * heartSize;
                foreach (AnimationSprite heart in hearts)
                {
                    heart.SetScaleXY(heartSize);
                    heart.SetXY(currentPosition.x, currentPosition.y);

                    currentPosition.x += spriteWidth;
                    if (currentPosition.x + spriteWidth > boundsMax.x)
                    {
                        currentPosition.x = boundsMin.x;
                        currentPosition.y += spriteHeight;
                    }
                }

                foreach (AnimationSprite heart in disappearingHearts)
                {
                    heart.SetScaleXY(heartSize);
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
