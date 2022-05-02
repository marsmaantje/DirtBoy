using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using Physics;
using TiledMapParser;
using Visuals;

namespace Objects
{
    public class PhysicalButton : ColliderObject
    {
        float pressDepth;
        MultiMover _mover;
        Vec2 up, released, pressed;
        bool isPressing = false;

        public PhysicalButton(TiledObject obj) : base(obj)
        {
            ReadVariables();
        }

        public PhysicalButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        {
            ReadVariables();
        }

        void ReadVariables()
        {
            pressDepth = obj.GetFloatProperty("pressDepth", 50f);
        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);

            //initialize moving parent
            _mover = new MultiMover();
            parent.AddChild(_mover);
            _mover.position = position;
            _mover.rotation = rotation;
            _mover.AddChild(this);

            SetXY(0, 0);
            rotation = 0;

            _mover.SetCollider(_collider);
            _collider.SetOwner(_mover);
            _mover.Mass = 10000; //set the mass very high so the player does not stick to the button
            _mover.Moving = true;
            _mover.OnCollision += OnCollision;

            _mover.AddChild(new MultiSegmentVisual(_collider));
            up = Vec2.GetUnitVectorDeg(_mover.rotation);
            released = _mover.position;
            pressed = released - (up * pressDepth);
        }

        public void Update()
        {
            _mover.Accelaration = -Mover.gravity;
            Vec2 onAxisVelocity = _mover.Velocity.Project(up) * 1000;
            if (!isPressing)
                onAxisVelocity = up * 0.01f;
            Vec2 newPosition = _mover.position + onAxisVelocity;
            _mover.position = newPosition.Clamp(pressed, released);
            _mover.Velocity = new Vec2();
            isPressing = false;
        }

        void OnCollision(Collider other, Mover current)
        {
            isPressing = true;
            //if(other.owner.HasChild(parentScene.player))
                //Console.WriteLine("hit with the player " + (_mover.Velocity * _mover.Mass));
        }
    }
}