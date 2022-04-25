using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Physics
{
    class Line : Collider
    {
		public Vec2 start;
		public Vec2 end;

		public Line(GameObject pOwner, Vec2 pStart, Vec2 pEnd) : base(pOwner, pStart)
		{
			start = pStart;
			end = pEnd;
		}

		/// <summary>
		/// Get the collision point of the ball with the given collider.
		/// </summary>
		/// <param name="other">other collider</param>
		/// <param name="velocity">delta position</param>
		/// <returns>earliest collision</returns>
		/// <exception cref="NotImplementedException"></exception>
		public override CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
		{
			if (other is Ball)
				return GetEarliestCollision((Ball)other, velocity);
			else if (other is Line)
				return GetEarliestCollision((Line)other, velocity);
			else
			{
				throw new NotImplementedException();
			}
		}

        CollisionInfo GetEarliestCollision(Ball other, Vec2 velocity)
        {
            CollisionInfo reverseCollision = other.GetEarliestCollision(this, -velocity);
            if (reverseCollision.timeOfImpact >= 0 && reverseCollision.timeOfImpact < 1)
            {
                return new CollisionInfo(-reverseCollision.normal, other, reverseCollision.timeOfImpact);
            }
            else
            {
                return new CollisionInfo(new Vec2(1,0), null, float.MaxValue);
            }
        }

        CollisionInfo GetEarliestCollision(Line other, Vec2 velocity)
        {
			return new CollisionInfo(new Vec2(1, 0), null, float.MaxValue);
        }

	}
}
