using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Physics;
using Visuals;

namespace GameElements
{
    class Bullet : Mover
    {

        public Bullet(Vec2 position, float radius, Vec2 velocity)
        {
            Collider c = new Ball(this, position, radius);
            SetCollider(c, false);
            BallVisual v = new BallVisual(radius);
            AddChild(v);
            v.SetXY(0, 0);
            Velocity = velocity;
        }

        public void Update()
        {
            Step();

            //destroy if outside screen
            if (y > game.height - 10)
            {
                SceneManager.main.removeBullet(this);
                LateDestroy();
            }
        }

        protected override void OnCollission(Collider other)
        {
            //if colliding with block, decrease health of block
            if(other.owner.parent is Block)
            {
                ((Block)other.owner.parent).Hit();
            }
        }
    }
}
