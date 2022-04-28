using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Physics;
using GXPEngine;

namespace Visuals
{
    public class MultiSegmentVisual : GameObject
    {
        MultiSegmentCollider collider;
        List<LineSegment> _segments;
        public List<LineSegment> Segments
        {
            get { return _segments; }
        }

        public MultiSegmentVisual(MultiSegmentCollider collider)
        {
            this.collider = collider;
            _segments = new List<LineSegment>();
            for (int i = 0; i < collider.segments.Count; i++)
            {
                _segments.Add(new LineSegment(collider.segments[i].start, collider.segments[i].end));
                _segments[i].SetXY(0,0);
                AddChild(_segments[i]);
            }
        }

        public void Update()
        {
            if(collider.segments.Count != _segments.Count)
            {
                RegenerateVisuals();
            }
            
            for (int i = 0; i < collider.segments.Count; i++)
            {
                _segments[i].start = collider.segments[i].start;
                _segments[i].end = collider.segments[i].end;
            }

            //Gizmos.DrawCross(0,0, 10, this);
        }

        void RegenerateVisuals()
        {
            foreach (LineSegment segment in _segments)
            {
                segment.LateDestroy();
            }
            _segments.Clear();
            for (int i = 0; i < collider.segments.Count; i++)
            {
                _segments.Add(new LineSegment(collider.segments[i].start, collider.segments[i].end, pShowNormal:true));
                _segments[i].SetXY(0, 0);
                AddChild(_segments[i]);
            }
        }
    }
}
