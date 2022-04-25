using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Physics;
using TiledMapParser;
using Visuals;

namespace Objects
{
    public class ComplexMover : CustomObject
    {
        MultiMover _mover;
        
        public ComplexMover(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider:false)
        {
            
        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            
            Move(-Origin());
            SetOrigin(0, 0);
            
            _mover = new MultiMover();
            parent.AddChild(_mover);
            _mover.position = position;
            _mover.rotation = rotation;
            _mover.AddChild(this);
            SetXY(0, 0);
            rotation = 0;
            MultiSegmentCollider col = ColliderLoader.main.GetCollider(obj.GetStringProperty("ColliderName"), _mover);
            _mover.SetCollider(col);
            
            _mover.AddChild(new MultiSegmentVisual(col));
            
        }

        public void Update()
        {
            _mover.Step();
        }
    }
}
