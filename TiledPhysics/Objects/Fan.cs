using System;
using GXPEngine;
using TiledMapParser;
using Physics;

namespace Objects
{
    public class Fan : CustomObject
    {
        Vec2 velocity;
        Vec2 areaOfEffect;
        
        float distance;
        float falloff;
        float strength;
        Vec2 direction;
        Vec2 checkVector;
        
        public Fan(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider: false)
        {
            
        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Vec2 windDirection = Vec2.GetUnitVectorDeg(rotation - 90);
            velocity = windDirection * obj.GetFloatProperty("windStrength", 1f);
            velocity *= windDirection;
            areaOfEffect = velocity * 5 - new Vec2(x,y);

            direction = Vec2.GetUnitVectorDeg(rotation);
            distance = obj.GetFloatProperty("distance", 10f);
            falloff = obj.GetFloatProperty("falloff", 0.5f);
            strength = obj.GetFloatProperty("strength", 1f);

            checkVector = direction * Mathf.Max(distance - height / 2, 0);
            Console.WriteLine(distance - height / 2);
            Console.WriteLine(distance);
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
                    //Console.WriteLine("mover in fan");
                    float force = strength * (1 - Mathf.Pow(distanceToFan / distance, 1 - falloff));
                    mover.ApplyForce(direction * force);
                    //Console.WriteLine(distanceToFan / distance);
                }
            }

            Gizmos.DrawArrow(position.x, position.y, checkVector.x, checkVector.y, 0.25f, parentScene);
        }
    }
}
