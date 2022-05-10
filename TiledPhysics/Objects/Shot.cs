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
        int deathMoment;
        
        public Shot(string filename, int cols, int rows, int pLifetime) : base(filename, cols, rows, null, false)
        {
            deathMoment = Time.time + pLifetime;
        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            _mover.OnCollision += Collision;
        }

        public void Update()
        {
            base.Update();
            if (Time.time > deathMoment)
            {
                LateDestroy();
            }
        }

        /// <summary>
        /// when we collide with a customobject, call its hit method and delete ourselves
        /// </summary>
        /// <param name="other"></param>
        /// <param name="current"></param>
        void Collision(Collider other, Mover current)
        {
            if(other.owner is CustomObject owner)
            {
                owner.OnHit(current, other);
            }
            LateDestroy();
        }
    }
}
