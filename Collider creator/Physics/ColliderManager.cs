using System;
using System.Collections.Generic;
using GXPEngine;

namespace Physics
{
	// This class holds our own colliders - similar to the GXPEngine's CollisionManager, but
	//  we can put any shape here.
	// Using MoveUntilCollision, you can move colliders (even those that aren't registered in this class!),
	//  while checking against collisions with the registered colliders. 
	// Using GetOverlaps you can get all overlapping trigger colliders that are registered.
	public class ColliderManager
	{
        /// <summary>
		/// endure there is always a general colliderManager
		/// </summary>
		public static ColliderManager main
		{
			get
			{
				if (_main == null)
				{
					_main = new ColliderManager();
				}
				return _main;
			}
		}

		static ColliderManager _main;

        //List of colliders
		public List<Collider> solidColliders;
		public List<Collider> triggerColliders;

		public ColliderManager()
		{
			solidColliders = new List<Collider>();
			triggerColliders = new List<Collider>();
		}

        /// <summary>
		/// Add a solid collider to the manager
		/// </summary>
		/// <param name="col"></param>
		public void AddSolidCollider(Collider col)
		{
			solidColliders.Add(col);
		}

        /// <summary>
        /// Remove a solid collider from the manager
        /// </summary>
        /// <param name="col"></param>
        public void RemoveSolidCollider(Collider col)
		{
			solidColliders.Remove(col);
		}

        /// <summary>
		/// Delete all dolis colliders
		/// </summary>
		public void ClearSolidColliders()
        {
			solidColliders = new List<Collider>();
        }

		/// <summary>
		/// Add a trigger collider to the manager
		/// </summary>
		/// <param name="col"></param>
		public void AddTriggerCollider(Collider col)
		{
			triggerColliders.Add(col);
		}

		/// <summary>
		/// Remove a trigger collider from the manager
		/// </summary>
		/// <param name="col"></param>
		public void RemoveTriggerCollider(Collider col)
		{
			triggerColliders.Remove(col);
		}

		/// <summary>
		/// Delete all trigger colliders
		/// </summary>
		public void ClearTriggerCollider()
        {
			triggerColliders = new List<Collider>();
        }

		// Note: feel free to do something smart here like space partitioning, to improve efficiency:
		//nah, not nessecary

		// Note that MoveUntilCollision checks against all solid colliders, but the given (moving) collider
		// does not need to be in that list.
		/// <summary>
		/// Try to move the given collider with the specified velocity.
		/// </summary>
		/// <param name="col">Collider to move</param>
		/// <param name="velocity">Velocity to move the collider with</param>
		/// <returns>Thing it collided with (if any)</returns>
		public CollisionInfo MoveUntilCollision(Collider col, Vec2 velocity, List<Collider> IgnoreList = null)
		{
            CollisionInfo firstCollision = GetEarliestCollision(col, velocity, IgnoreList);
            // Given the earliest time of impact, move to the point of impact:
            float TOI = 1;
			if (firstCollision != null && firstCollision.timeOfImpact < 1 && firstCollision.timeOfImpact >= 0)
			{
				TOI = firstCollision.timeOfImpact;
			}
			col.position += velocity * TOI;
			return firstCollision;
		}

        /// <summary>
		/// Get the earliest collision with any solid collider
		/// </summary>
		/// <param name="col">Collider we are moving</param>
		/// <param name="velocity">Amount we are moving it</param>
		/// <param name="IgnoreList">Colliders we should ignore for our collision check</param>
		/// <returns>First collision</returns>
        public CollisionInfo GetEarliestCollision(Collider col, Vec2 velocity, List<Collider> IgnoreList = null)
        {
            CollisionInfo firstCollision = null;
            foreach (Collider other in solidColliders)
            {
                if (other != col && (IgnoreList == null || !IgnoreList.Contains(other)))
                {
                    CollisionInfo colInfo = col.GetEarliestCollision(other, velocity);
                    if (colInfo != null && colInfo.timeOfImpact < 1)
                    {
                        if (firstCollision == null || firstCollision.timeOfImpact > colInfo.timeOfImpact)
                        {
                            firstCollision = colInfo;
                        }
                    }
                }
            }
            return firstCollision;
        }

        // Note that GetOverlaps checks against all trigger colliders by default, but the given collider
        // does not need to be in that list.
        /// <summary>
        /// Check all colliders we are currently overlapping with
        /// </summary>
        /// <param name="col">Collider to check overlaps with</param>
        /// <param name="staticCollisions">Should we check static colliders?</param>
        /// <param name="triggerCollisions">Should we check trigger colliders?</param>
        /// <returns>List of all colliders we are colliding with</returns>
        public List<Collider> GetOverlaps(Collider col, bool staticCollisions = false, bool triggerCollisions = true)
		{
			List<Collider> overlaps = new List<Collider>();
			if (triggerCollisions)
			{
				foreach (Collider other in triggerColliders)
				{
					if (other != col && col.Overlaps(other))
					{
						overlaps.Add(other);
					}
				}
			}
			if(staticCollisions)
            {
				foreach (Collider other in solidColliders)
				{
					if (other != col && col.Overlaps(other))
					{
						overlaps.Add(other);
					}
				}
			}
			return overlaps;
		}
	}
}