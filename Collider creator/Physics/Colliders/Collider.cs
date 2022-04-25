using System;
using GXPEngine;

namespace Physics
{
	/// <summary>
	/// Base collider class, this should be implemented by specific colliders and not used on its own
	/// </summary>
    public class Collider
    {
		public GameObject owner;
		public Vec2 position;
		public bool draw = true;

		public Collider(GameObject pOwner, Vec2 startPosition)
		{
			owner = pOwner;
			position = startPosition;
		}

		// Implement this method in subclasses:
		public virtual CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
		{
			throw new NotImplementedException();
		}

		// Implement this method in subclasses:
		public virtual bool Overlaps(Collider other)
		{
			throw new NotImplementedException();
		}
	}
}
