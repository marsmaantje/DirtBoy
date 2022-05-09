using System;
using GXPEngine;
using GXPEngine.Core;
using Objects;

namespace Particles
{
    class ParticleEmitter : CustomObject
    {
        ParticleStyle particleStyle;
        public float rate, spreadAngle;
        float spawnBucket;
        public string sprite;
        public Vec2 direction;
        public GameObject particleSpace;

        public ParticleEmitter(string pSprite)
        {
            particleStyle = new ParticleStyle();
            sprite = pSprite;
            particleSpace = this;
        }
        void spawnParticle()
        {
            float angle = direction.GetAngleDegrees();
            angle = Utils.Random(angle - spreadAngle, angle + spreadAngle);
            float speed = Utils.Random(particleStyle.minSpeed, particleStyle.maxSpeed);
            Vec2 initialVelocity = new Vec2();
            initialVelocity.setAngleLength(Vec2.Deg2Rad(angle), speed);

            Particle particle = new Particle(sprite, particleStyle, initialVelocity);
            particleSpace.AddChild(particle);

        }
        public void Update()
        {
            spawnBucket += rate;
            while (spawnBucket > 1)
            {
                spawnParticle();
                spawnBucket--;
            }
        }

        public void Burst(float amount) => spawnBucket += amount;
        
    }
}