using System;
using GXPEngine;
using TiledMapParser;
using Physics;

namespace Objects
{
    public class Trampoline : Button
    {
        Vec2 velocity;

        public Trampoline(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        {

        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Vec2 windDirection = Vec2.GetUnitVectorDeg(rotation);
            velocity += windDirection * obj.GetFloatProperty("strength", 0f);
            OnPressing += pushPlayer;
            OnPressed += pushPlayer;
        }

        void pushPlayer()
        {
            parentScene.player.Mover.Velocity = velocity;
        }
    }
}
