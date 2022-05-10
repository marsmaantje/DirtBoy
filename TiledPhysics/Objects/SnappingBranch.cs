using System;
using GXPEngine;
using TiledMapParser;
using Physics;

namespace Objects
{
    public class SnappingBranch : CustomObject
    {
        int forceRequired;
        bool falling;
        Vec2 velocity;

        public SnappingBranch(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider: false)
        {

        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            forceRequired = obj.GetIntProperty("snappingForce", 10);
            velocity = new Vec2(0, 0);
        }

        public void Update()
        {
            x += velocity.x;
            y += velocity.y;
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
                velocity.y = 10;
            }
        }
    }
}
