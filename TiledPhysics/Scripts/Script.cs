using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
using Objects;

namespace Scripts
{
    class Script : CustomObject
    {
        public Script(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider: true)
        {
            collider.isTrigger = true;
            visible = false;

        }

        public Script() : base()
        { }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Console.WriteLine("baseScript initialized");


        }
    }
}
