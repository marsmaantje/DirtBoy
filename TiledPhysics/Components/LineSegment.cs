using System;
using GXPEngine.Core;
using GXPEngine.OpenGL;

namespace GXPEngine
{
	/// <summary>
	/// Implements an OpenGL line
	/// </summary>
	public class LineSegment : GameObject
	{
		public Vec2 start;
		public Vec2 end;

		public uint color = 0xffffffff;
		public uint lineWidth = 1;

		bool showNormal = false;

		public LineSegment (float pStartX, float pStartY, float pEndX, float pEndY, uint pColor = 0xffffffff, uint pLineWidth = 1, bool showNormal = false)
			: this (new Vec2 (pStartX, pStartY), new Vec2 (pEndX, pEndY), pColor, pLineWidth, showNormal)
		{ }
		
        
		public LineSegment (Vec2 pStart, Vec2 pEnd, uint pColor = 0xffffffff, uint pLineWidth = 1, bool pShowNormal = false)
		{
			start = pStart;
			end = pEnd;
			color = pColor;
			lineWidth = pLineWidth;
            showNormal = pShowNormal;
        }
	
		//------------------------------------------------------------------------------------------------------------------------
		//														RenderSelf()
		//------------------------------------------------------------------------------------------------------------------------
		override protected void RenderSelf(GLContext glContext) {
			if (game != null) {
                Gizmos.DrawLine(start.x, start.y, end.x, end.y, this,  color, (byte)lineWidth);
                //Gizmos.RenderLine(start.x, start.y, end.x, end.y, color, lineWidth);
                if(showNormal)
                {
                    Vec2 middle = (start + end) / 2;
                    Vec2 normal = (end - start).Normal();
                    Gizmos.DrawArrow(middle.x, middle.y, normal.x * 10, normal.y * 10, space:this, color: 0xffff0000, width:1);
                }
			}
		}
	}
}

