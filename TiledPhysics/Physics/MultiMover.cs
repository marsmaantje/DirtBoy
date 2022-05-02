using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Physics
{
    public class MultiMover : Mover
    {

        List<Collider> _colliders;
        MultiSegmentCollider _multiCollider;

        public MultiMover()
        {
            _colliders = new List<Collider>();
        }

        public void SetCollider(MultiSegmentCollider multiCollider, bool addToManager = true)
        {
            _multiCollider = multiCollider;
            _colliders = _multiCollider.GetColliders();
            if (addToManager)
                multiCollider.AddToManager(ColliderManager.main);
        }

        public override void Step()
        {
            Velocity += Accelaration;
            Velocity += gravity;

            //if we have colliders to move
            if (_colliders != null)
            {
                CollisionInfo firstCollision = GetEarliestCollision();
                UpdatePosition(firstCollision);
                lastCollision = firstCollision;

                if (firstCollision != null && Mathf.RoughlyEquals(firstCollision.timeOfImpact, 0, 0.001f))
                {
                    firstCollision = GetEarliestCollision();
                    UpdatePosition(firstCollision);
                    lastCollision = firstCollision ?? lastCollision;
                    _velocity *= 1 - _friction;
                }
                else
                {
                    _velocity *= 1 - _airFriction;
                }
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
                position += Velocity;
            else
            {
                position += Velocity * info.timeOfImpact;
                ResolveCollision(info);

                if (info.other != null)
                {
                    Collision(info.other); //call the collission event to perhaps do something with the collission
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

            }
            else
            {//collission with immovable object
                _velocity.Reflect(info.normal, Bounciness);
            }
        }

        /// <summary>
        /// Get the earliest collision that we have to resolve
        /// </summary>
        /// <returns></returns>
        CollisionInfo GetEarliestCollision()
        {
            ColliderManager manager = ColliderManager.main;
            CollisionInfo FirstCollision = null;
            foreach (Collider collider in _colliders)
            {
                CollisionInfo col = manager.GetEarliestCollision(collider, Velocity, _colliders);
                if (FirstCollision == null || (col != null && col.timeOfImpact < FirstCollision.timeOfImpact))
                {
                    FirstCollision = col;
                }
            }

            return FirstCollision;
        }
    }
}
