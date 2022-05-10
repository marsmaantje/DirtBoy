using System;
using GXPEngine;
using TiledMapParser;
using Physics;

namespace Objects
{
    public class Trampoline : Button
    {
        float strength;
        Vec2 velocity, checkVector;

        public Trampoline(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        {

        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Vec2 windDirection = Vec2.GetUnitVectorDeg(rotation);
            velocity += windDirection * obj.GetFloatProperty("strength", 1f);
        }

        public void Update()
        {

        }
    }
}
