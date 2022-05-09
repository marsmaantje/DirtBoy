using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using GXPEngine.Core;
using GXPEngine.OpenGL;

namespace Particles
{
    class Particle : Sprite
    {
        ParticleStyle style;
        float size;
        float direction;
        int initializationTime, lifespan;
        Vec2 velocity;
        public Particle(string sprite, ParticleStyle pStyle, Vec2 initialVelocity) : base(sprite, false, false)
        {
            style = pStyle;
            Initialize();
            initializationTime = Time.time;
            velocity = initialVelocity;
        }
        void Initialize()
        {
            size = Utils.Random(style.minSize, style.maxSize);
            lifespan = (int) (Utils.Random(style.minLifeTime, style.maxLifeTime) * 1000);
            alpha = Utils.Random(style.minAlpha, style.maxAlpha);
        }
        void Update()
        {
            if (initializationTime + lifespan < Time.time)
            {
                LateDestroy();
            }
            else
            {
                velocity += style.gravity;
                position += velocity;
            }
        }
    }
}