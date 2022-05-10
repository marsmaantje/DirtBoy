using System;
using GXPEngine;
using TiledMapParser;
using Physics;

namespace Objects
{
    public class SnappingBranch : ColliderObject
    {
        int forceRequired;
        bool falling;

        float fallProgress, fallDuration, startX, endX, startY, endY, startRot, endRot;

        public SnappingBranch(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj, addCollider: false, pAddToManager:false)
        {

        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            forceRequired = obj.GetIntProperty("snappingForce", 10);
            startX = x;
            startY = y;
            startRot = rotation;
            endX = x + obj.GetFloatProperty("xDelta", 0f);
            endY = y + obj.GetFloatProperty("yDelta", 0f);
            endRot = rotation + obj.GetFloatProperty("rotDelta", 0f);
            fallDuration = obj.GetFloatProperty("fallDuration", 1f);
        }

        public void Update()
        {
            if(falling)
            {
                if (fallProgress >= 1)
                    falling = false;
                else
                {
                    fallProgress += 1f / 60f / fallDuration;
                    fallProgress = Mathf.Clamp(fallProgress, 0, 1);
                    x = Mathf.lerp(startX, endX, fallProgress);
                    y = Mathf.lerp(startY, endY, fallProgress);
                    rotation = Mathf.lerp(startRot, endRot, fallProgress);
                }
            }
        }

        public override void OnHit(Mover source, Collider collider)
        {
            float force = 0.5f * Mathf.Pow(source.Velocity.Length(), 2);
            if (source.HasChild(parentScene.player))
            {
                force *= parentScene.player.health;
            }
            if (falling || force > forceRequired)
            {
                falling = true;
            }
        }
    }
}
