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
        protected SingleMover _mover;
        bool addToManager;

        public Mover Mover
        {
            get => _mover;
        }

        public PhysicsBall(string filename, int cols, int rows, TiledObject obj, bool addToManager = true) : base(obj, filename, cols, rows, addCollider: false)
        {
            this.addToManager = addToManager;
        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            _mover = new SingleMover();
            parent.AddChild(_mover);
            _mover.position = position;
            _mover.AddChild(this);
            SetXY(0, 0);
            _mover.SetCollider(new Ball(_mover, _mover.position, 10), addToManager);
        }

        public void Update()
        {
            _mover.Step();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _mover.Destroy();
        }
    }
}
