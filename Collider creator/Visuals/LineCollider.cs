using System;
using GXPEngine.Core;
using GXPEngine.OpenGL;
using GXPEngine;
using Physics;

namespace Visuals
{
	/// <summary>
	/// Implements an OpenGL line
	/// </summary>
	public class LineCollider : GameObject
	{
		public Vec2 start;
		public Vec2 end;

		Line lineCollider;
		Line lineColliderReverse;
		Ball startCollider;
		Ball endCollider;

		public uint color = 0xffffffff;
		public uint lineWidth = 1;

		public LineCollider (float pStartX, float pStartY, float pEndX, float pEndY, uint pColor = 0xffffffff, uint pLineWidth = 1)
			: this (new Vec2 (pStartX, pStartY), new Vec2 (pEndX, pEndY), pColor, pLineWidth)
		{
		}

		public LineCollider (Vec2 pStart, Vec2 pEnd, uint pColor = 0xffffffff, uint pLineWidth = 1)
		{
			start = pStart;
			end = pEnd;
			color = pColor;
			lineWidth = pLineWidth;

			ColliderManager manager = ColliderManager.main;

			lineCollider = new Line(this, start, end);
			lineColliderReverse = new Line(this, end, start);
			startCollider = new Ball(this, start, 0);
			endCollider = new Ball(this, end, 0);
			manager.AddSolidCollider(lineCollider);
			manager.AddSolidCollider(lineColliderReverse);
			manager.AddSolidCollider(startCollider);
			manager.AddSolidCollider(endCollider);
		}

		public void Update()
        {
			//make sure the collider stays in place with the visual
			if (startCollider.position != start)
			{
				startCollider.position = start;
				lineCollider.start = start;
				lineColliderReverse.end = start;
			}
			if (endCollider.position != end)
			{
				endCollider.position = end;
				lineCollider.end = end;
				lineColliderReverse.start = end;
			}
        }

        /// <summary>
        /// Set a new position of the line and update the colliders
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void setStartEnd(Vec2 start, Vec2 end)
        {
			this.start = start;
			this.end = end;
        }

        /// <summary>
		/// Remove the colliders from the manager when we are destroyed
		/// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
			ColliderManager manager = ColliderManager.main;

			manager.RemoveSolidCollider(lineCollider);
			manager.RemoveSolidCollider(lineColliderReverse);
			manager.RemoveSolidCollider(startCollider);
			manager.RemoveSolidCollider(endCollider);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RenderSelf()
        //------------------------------------------------------------------------------------------------------------------------
        override protected void RenderSelf(GLContext glContext) {
			if (game != null) {
				Gizmos.RenderLine(start.x, start.y, end.x, end.y, color, lineWidth);
			}
		}
	}
}

