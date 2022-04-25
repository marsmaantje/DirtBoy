using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Physics
{
    public class Mover : Pivot
    {
        public static bool drawDebugLine = false;

        public Collider collider;

        /// <summary>
        /// Get and set the local position of the mover
        /// </summary>
        public Vec2 position
        {
            get => new Vec2(x, y);
            set
            {
                SetXY(value.x, value.y);
                collider.position = value;
            }
        }

        /// <summary>
        /// Get and set the global position of the block
        /// </summary>
        public Vec2 globalPosition
        {
            get => new Vec2(TransformPoint(0, 0));
            set => collider.position = new Vec2(InverseTransformPoint(value.x, value.y));
        }

        public Vec2 Velocity = new Vec2(0, 0);
        public Vec2 Accelaration = new Vec2(0, 0);
        public static Vec2 gravity = new Vec2(0,0);
        public float Mass = 1;
        public float Bounciness = 1f;
        public bool moving = true;

        public Mover()
        {

        }

        /// <summary>
        /// Set the collider of the mover
        /// </summary>
        /// <param name="c">Collider</param>
        /// <param name="addToManager">Should we add it to the manager so others can collide with us?</param>
        public void SetCollider(Collider c, bool addToManager = true)
        {
            if (collider != null)
                ColliderManager.main.RemoveSolidCollider(collider);

            collider = c;
            if(addToManager)
                ColliderManager.main.AddSolidCollider(collider);
        }

        /// <summary>
        /// Forwards the mover by one step and does all the collission calculations
        /// </summary>
        public void Step()
        {
            Velocity += Accelaration;
            Velocity += gravity;

            ColliderManager manager = ColliderManager.main;
            CollisionInfo firstCollision = manager.MoveUntilCollision(collider, Velocity);

            UpdatePosition(firstCollision);
            if(firstCollision != null && Mathf.RoughlyEquals(firstCollision.timeOfImpact, 0, 0.001f))
            {
                firstCollision = manager.MoveUntilCollision(collider, Velocity);
                UpdatePosition(firstCollision);
            }

            
        }

        /// <summary>
        /// Update the position of the mover with the given collision info.
        /// Needed since MoveUntillCollision only moves the collider, not the Mover
        /// </summary>
        /// <param name="info"></param>
        void UpdatePosition(CollisionInfo info)
        {
            if(info == null)
                position = collider.position;
            else
            {
                position = collider.position;
                ResolveCollision(info);



                if (info.other != null)
                {
                    OnCollission(info.other); //call the collission event to perhaps do something with the collission
                    if (info.other.owner is Mover)
                        ((Mover)info.other.owner).OnCollission(this.collider);
                }
            }
        }

        /// <summary>
        /// Resolve the collision.
        /// Update velocity according to what we hit
        /// </summary>
        /// <param name="info"></param>
        void ResolveCollision(CollisionInfo info)
        {
            info.normal.Normalize();
            if(info.other.owner is Mover && ((Mover)info.other.owner).moving)
            {//collission with movable object
                Mover other = (Mover)info.other.owner;
                Vec2 normal = info.normal;

                Vec2 systemVelocity = (Velocity * Mass + other.Velocity * other.Mass) / (Mass + other.Mass);

                Vec2 vel1 = Velocity - systemVelocity;
                Vec2 vel2 = other.Velocity - systemVelocity;
                Velocity = systemVelocity + vel1.Reflect(normal, Bounciness);
                other.Velocity = systemVelocity + vel2.Reflect(normal, Bounciness);

                //position += info.normal * 1f;//move away from the other so it doesnt collide again in their step

            }
            else
            {//collission with immovable object
                Velocity.Reflect(info.normal, Bounciness);

                //position += info.normal * .01f;//move away from the other so it doesnt collide again in their step
            }
        }

        /// <summary>
        /// Method should be overridden by subclass
        /// </summary>
        /// <param name="other">This we collided with</param>
        virtual protected void OnCollission(Collider other)
        { }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(collider != null)
                ColliderManager.main.RemoveSolidCollider(collider);
        }

    }
}
