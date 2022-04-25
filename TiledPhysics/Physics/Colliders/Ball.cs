using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Physics
{
    public class Ball : Collider
    {
        public readonly float radius;

        public Ball(GameObject pOwner, Vec2 startPosition, float radius) : base(pOwner, startPosition)
        {
            this.radius = radius;
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
			if (other is AABB)
				return GetEarliestCollision((AABB)other, velocity);
			else if (other is Ball)
				return GetEarliestCollision((Ball)other, velocity);
			else if (other is Line)
				return GetEarliestCollision((Line)other, velocity);
			else
			{
				throw new NotImplementedException();
			}
		}

        /// <summary>
        /// Get the collision point of the ball with the given AABB.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        CollisionInfo GetEarliestCollision(AABB other, Vec2 velocity)
		{
			throw new NotImplementedException();
		}

        /// <summary>
		/// Get the collision point of the ball with the given ball.
		/// </summary>
		/// <param name="other"></param>
		/// <param name="velocity"></param>
		/// <returns></returns>
		CollisionInfo GetEarliestCollision(Ball other, Vec2 velocity)
        {
			Vec2 relativePosition = position - other.position;
			float a = Mathf.Pow(velocity.Length(), 2);
			float b = relativePosition.Dot(velocity) * 2;
			float c = Mathf.Pow(relativePosition.Length(), 2) - Mathf.Pow(radius + other.radius, 2);
			float d = (b * b) - (4 * a * c);

			if (d < 0) //no collission
				return new CollisionInfo(new Vec2(1, 0), null, float.MaxValue);

			float TOI = (-b - Mathf.Sqrt(d)) / (2 * a);

			relativePosition = (position + (velocity * TOI)) - other.position;
			Vec2 normal = relativePosition.Normalized();

			// check sign of b too b pos, c neg: dont do anything (=prevent sticky ballz) DONE (hopefully)
			if (c < 0)
			{
				if (b < 0)
					return new CollisionInfo(normal, other, 0);
				else
					return new CollisionInfo(new Vec2(1, 0), null, float.MaxValue);
			}




			if (0 <= TOI && TOI < 1)
				return new CollisionInfo(normal, other, TOI);
			else //no collission
				return new CollisionInfo(new Vec2(1, 0), null, float.MaxValue);
		}

        /// <summary>
		/// Get the collision point of the ball with the given line.
		/// </summary>
		/// <param name="other"></param>
		/// <param name="velocity"></param>
		/// <returns></returns>
		CollisionInfo GetEarliestCollision(Line other, Vec2 velocity)
		{
			Vec2 point = position - other.start; //ball position relative to line
			Vec2 line = other.end - other.start; //vector of the line
			Vec2 lineNormal = line.Normal(); //normal of the line

			Vec2 projected = point.Project(line);//.Clamp(new Vec2(0,0), line); //nearest point on line to ball -> hm...

			float b = -velocity.Dot(lineNormal);
			float a = Vec2.Distance(projected, point) - radius;
			if(point.Dot(line.Normal()) < 0)
			{//underneath the line, ignore
				return new CollisionInfo(new Vec2(1, 0), null, float.MaxValue);
			}

			float t;
			if (b <= 0)
				return new CollisionInfo(new Vec2(1, 0), null, float.MaxValue);
            
			if (a >= 0)
				t = a / b;
			else if (a >= -radius)
				t = 0;
			else // check the pseudocode (fallthrough prevention) DONE (hopefully)
				return new CollisionInfo(new Vec2(1, 0), null, float.MaxValue);

			if (t <= 1)
			{
				Vec2 POI = (position - other.start) + velocity * t;
				float d = POI.Dot(line.Normalized());
				if (d >= 0 && d <= line.Length())
					return new CollisionInfo(lineNormal, other, t);
			}

			return new CollisionInfo(new Vec2(1, 0), null, float.MaxValue);

		}
	}
}
