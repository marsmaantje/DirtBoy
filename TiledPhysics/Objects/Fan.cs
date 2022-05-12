using System;
using GXPEngine;
using TiledMapParser;
using Physics;

namespace Objects
{
    public class Fan : CustomObject
    {
        Vec2 areaOfEffect;
        
        float distance;
        float falloff;
        float strength;
        Vec2 direction;
        Vec2 checkVector;
        
        public Fan(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider: false)
        {
            visible = false;
        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            direction = Vec2.GetUnitVectorDeg(rotation);
            distance = obj.GetFloatProperty("distance", 10f);
            falloff = obj.GetFloatProperty("falloff", 0.5f);
            strength = obj.GetFloatProperty("strength", 1f);

            checkVector = direction * Mathf.Max(distance - height / 2, 0);
        }

        public void Update()
        {
            foreach(Mover mover in parentScene.FindObjectsOfType<Mover>())
            {
                Vec2 relativePosition = mover.position - position;
                float distanceToCenter = Vec2.PointToLineDistance(relativePosition, checkVector);
                float distanceToFan = relativePosition.Dot(direction);
                if (distanceToCenter < height/2 && distanceToFan < distance && distanceToFan > 0)
                {//if the mover is inside the area of effect
                    float force = strength * (1 - Mathf.Pow(distanceToFan / distance, 1 - falloff));
                    mover.ApplyForce(direction * force);
                }
            }
        }
    }
}
