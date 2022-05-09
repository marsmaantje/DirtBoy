using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using Physics;

namespace Objects
{
    public class Shot : PhysicsBall
    {
        public Shot(string filename, int cols, int rows) : base(filename, cols, rows, null)
        {
            
        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            _mover.OnCollision += Collision;
        }

        void Collision(Collider other, Mover current)
        {
            
        }
    }
}
