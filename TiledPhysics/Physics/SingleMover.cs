using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Physics
{
    public class SingleMover : Mover
    {
        public Collider collider;

        public SingleMover() : base()
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
            if (addToManager)
                ColliderManager.main.AddSolidCollider(collider);
        }

        /// <summary>
        /// Forwards the mover by one step and does all the collission calculations
        /// </summary>
        override public void Step()
        {
            Velocity += Accelaration;
            Velocity += gravity;

            ColliderManager manager = ColliderManager.main;
            CollisionInfo firstCollision = manager.MoveUntilCollision(collider, Velocity);

            UpdatePosition(firstCollision);
            if (firstCollision != null && Mathf.RoughlyEquals(firstCollision.timeOfImpact, 0, 0.001f))
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
            if (info == null)
                position = collider.position;
            else
            {
                position = collider.position;
                ResolveCollision(info);



                if (info.other != null)
                {
                    Collision(info.other); //call the collission event to perhaps do something with the collission
                    if (info.other.owner is Mover)
                        ((Mover)info.other.owner).Collision(collider);
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
            if (info.other.owner is Mover && ((Mover)info.other.owner).Moving)
            {//collission with movable object
                Mover other = (Mover)info.other.owner;
                Vec2 normal = info.normal;

                Vec2 systemVelocity = (Velocity * Mass + other.Velocity * other.Mass) / (Mass + other.Mass);

                Vec2 vel1 = Velocity - systemVelocity;
                Vec2 vel2 = other.Velocity - systemVelocity;
                _velocity = systemVelocity + vel1.Reflect(normal, Bounciness);
                other.Velocity = systemVelocity + vel2.Reflect(normal, Bounciness);

                //position += info.normal * 1f;//move away from the other so it doesnt collide again in their step

            }
            else
            {//collission with immovable object
                _velocity.Reflect(info.normal, Bounciness);

                //position += info.normal * .01f;//move away from the other so it doesnt collide again in their step
            }
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (collider != null)
                ColliderManager.main.RemoveSolidCollider(collider);
        }
    }
}
