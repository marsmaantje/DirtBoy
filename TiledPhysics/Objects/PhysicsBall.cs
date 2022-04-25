using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Physics;
using TiledMapParser;

namespace Objects
{
    public class PhysicsBall : CustomObject
    {
        SingleMover _mover;
        
        public PhysicsBall(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider:false)
        {
            
        }

        public override void initialize(Scene parentScene)
        {
            Console.WriteLine(InverseTransformPoint(0,0));
            base.initialize(parentScene);
            _mover = new SingleMover();
            parent.AddChild(_mover);
            _mover.position = position;
            _mover.AddChild(this);
            SetXY(0, 0);
            _mover.SetCollider(new Ball(_mover, _mover.position, 10));
            Console.WriteLine(InverseTransformPoint(0,0));
        }

        public void Update()
        {
            _mover.Step();
        }
    }
}
