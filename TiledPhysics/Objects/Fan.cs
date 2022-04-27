using System;
using GXPEngine;
using TiledMapParser;

namespace Objects
{
    public class Fan : CustomObject
    {
        Vec2 velocity;
        Vec2 areaOfEffect;
        public Fan(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider: true)
        {
            collider.isTrigger = true;
        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Vec2 windDirection = Vec2.GetUnitVectorDeg(rotation - 90);
            velocity = windDirection * obj.GetFloatProperty("windStrength", 1f);
            velocity *= windDirection;
            areaOfEffect = velocity * 5 - new Vec2(x,y);
        }
        void OnCollision(GameObject other)
        {
            if (other is EasyDraw easyDraw)
            {
                if (easyDraw.parent is Player player)
                {
                    Console.WriteLine("player got hit");
                }
            }
        }
    }
}
