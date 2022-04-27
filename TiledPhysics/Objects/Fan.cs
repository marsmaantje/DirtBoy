using System;
using TiledMapParser;

namespace Objects
{
    public class Fan : CustomObject
    {
        Vec2 velocity;
        public Fan(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider: true)
        {
            collider.isTrigger = true;
        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Vec2 windDirection = Vec2.GetUnitVectorDeg(rotation);
            velocity = windDirection * obj.GetFloatProperty("windStrength", 1f);
            velocity *= windDirection;
        }
    }
}
