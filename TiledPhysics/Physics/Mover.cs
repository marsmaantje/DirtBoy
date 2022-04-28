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
        public event OnCollision OnCollision;
        public CollisionInfo lastCollision = null;


        /// <summary>
        /// Get and set the global position of the block
        /// </summary>
        public Vec2 globalPosition
        {
            get => new Vec2(TransformPoint(0, 0));
            set => position = InverseTransformPoint(value);
        }

        #region Variables

        /// <summary>
        /// The velocity of the object
        /// </summary>
        protected Vec2 _velocity = new Vec2(0, 0);
        public Vec2 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
            }
        }

        /// <summary>
        /// The accaleration of the object
        /// </summary>
        protected Vec2 _accelaration = new Vec2(0, 0);
        public Vec2 Accelaration
        {
            get => _accelaration;
            set
            {
                _accelaration = value;
            }
        }

        public static Vec2 gravity = new Vec2(0,0);

        /// <summary>
        /// The mass of the object
        /// </summary>
        protected float _mass = 1;
        public float Mass
        {
            get => _mass;
            set
            {
                _mass = value;
            }
        }

        /// <summary>
        /// The bounciness of the object
        /// </summary>
        protected float _bounciness = 0.95f;
        public float Bounciness
        {
            get => _bounciness;
            set
            {
                _bounciness = value;
            }
        }

        /// <summary>
        /// Does the object move in the next frame
        /// </summary>
        protected bool _moving = true;
        public bool Moving
        {
            get => _moving;
            set
            {
                _moving = value;
            }
        }

        /// <summary>
        /// The friction of the object, gets applied when on the ground
        /// </summary>
        protected float _friction = 0.03f;
        public float Friction
        {
            get => _friction;
            set
            {
                _friction = value;
            }
        }

        /// <summary>
        /// Friction of the oject in the air
        /// </summary>
        protected float _airFriction = 0.005f;
        public float AirFriction
        {
            get => _airFriction;
            set
            {
                _airFriction = value;
            }
        }

        #endregion

        public Mover()
        {
            OnCollision += (Collider other, Mover current) => { };
        }

        /// <summary>
        /// Applies the given force on the velocity
        /// </summary>
        /// <param name="force">The force to apply</param>
        public void ApplyForce(Vec2 force)
        {
            _velocity += force / _mass;
        }

        /// <summary>
        /// Forwards the mover by one step and does all the collission calculations
        /// </summary>
        public virtual void Step()
        { }
        
        public void Collision(Collider other)
        {
            OnCollision(other, this);
        }
    }

    public delegate void OnCollision(Collider other, Mover current);
}
