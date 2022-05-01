using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Physics
{
    public class MultiSegmentCollider
    {
        GameObject _owner;
        readonly List<Segment> _segments;
        public List<Segment> segments
        {
            get => _segments;
        }

        Vec2 _position;
        public Vec2 Position
        {
            get => _position;
        }
        float _rotation;

        public MultiSegmentCollider(GameObject owner)
        {
            _segments = new List<Segment>();
            _owner = owner;
            this._position = owner.position;
            this._rotation = owner.rotation;

            //add the Update to the update loop
            Game.main.OnBeforeStep += Update;
        }

        /// <summary>
        /// Add segment to the collider
        /// </summary>
        /// <param name="start">start position</param>
        /// <param name="end">end position</param>
        /// <param name="endCap">add ball collider to the end</param>
        public void AddSegment(Vec2 start, Vec2 end, bool endCap = false)
        {
            _segments.Add(new Segment(_owner, start, end, _position, _rotation, endCap));
        }

        /// <summary>
        /// Remove a segment from the collider
        /// </summary>
        /// <param name="index">segment to remove</param>
        public void RemoveSegment(int index)
        {
            _segments.RemoveAt(index);
        }

        /// <summary>
        /// Remove the las segment from the list
        /// </summary>
        public void RemoveLastSegment() => RemoveSegment(_segments.Count - 1);

        /// <summary>
        /// Set the owner to a new one
        /// </summary>
        /// <param name="owner">the new owner</param>
        public void setOwner(GameObject owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Add all segments to the colliderManager
        /// </summary>
        /// <param name="mananger">manager to add the segments to</param>
        /// <param name="trigger">add to the triggerCollider list instead of Solid</param>
        public void AddToManager(ColliderManager manager, bool trigger = false)
        {
            foreach (Segment segment in segments)
            {
                segment.addToManager(manager, trigger);
            }
        }

        /// <summary>
        /// Remove all segments from the collider manager
        /// </summary>
        /// <param name="manager">colliders to remove</param>
        public void RemoveFromManager(ColliderManager manager, bool isTrigger = false)
        {
            foreach (Segment segment in segments)
            {
                segment.RemoveFromManager(manager, isTrigger);
            }
        }

        /// <summary>
        /// Updates before everything else
        /// check if the position of our owner has changed, if so, update the position of everything
        /// </summary>
        public void Update()
        {
            if (_position != _owner.position || _rotation != _owner.rotation)
            {
                _position = _owner.position;
                _rotation = _owner.rotation;
                foreach (Segment segment in _segments)
                {
                    segment.Update(_position, _rotation);
                }
            }
        }

        public void GetBounds(out Vec2 center, out Vec2 size)
        {
            Vec2 min = _segments[0].start;
            Vec2 max = _segments[0].start;
            foreach (Segment segment in _segments)
            {
                if (segment.start.x < min.x) min.x = segment.start.x;
                if (segment.start.y < min.y) min.y = segment.start.y;
                if (segment.end.x < min.x) min.x = segment.end.x;
                if (segment.end.y < min.y) min.y = segment.end.y;
                if (segment.start.x > max.x) max.x = segment.start.x;
                if (segment.start.y > max.y) max.y = segment.start.y;
                if (segment.end.x > max.x) max.x = segment.end.x;
                if (segment.end.y > max.y) max.y = segment.end.y;
            }

            center = (min + max) / 2;
            size = max - min;
        }

        /// <summary>
        /// Get all colliders of this object
        /// </summary>
        /// <returns>List of all colliders</returns>
        public List<Collider> GetColliders()
        {
            List<Collider> colliders = new List<Collider>();
            foreach (Segment segment in _segments)
            {
                colliders.AddRange(segment.GetColliders());
            }
            return colliders;
        }
        public override string ToString()
        {
            string ret = "[";
            foreach (Segment segment in segments)
            {
                ret += segment.ToString();
                ret += ", ";
            }
            ret += "]";

            return ret;
        }
    }

    /// <summary>
    /// part of a multisegmentCollider
    /// manages a line and one or two balls
    /// </summary>
    public class Segment
    {
        readonly Line line;
        readonly Ball ball1;
        readonly Ball ball2;
        public Vec2 start; //start of the line
        public Vec2 end;   //end of the line
        GameObject _owner;

        public Segment(GameObject owner, Vec2 start, Vec2 end, Vec2 offset, float rotation, bool dualSidedBalls = false)
        {
            _owner = owner;

            this.start = start;
            this.end = end;

            //create the colliders, we will position them later
            if (dualSidedBalls)
            {
                ball1 = new Ball(_owner, new Vec2(), 0);
                ball2 = new Ball(_owner, new Vec2(), 0);
            }
            else
            {
                ball1 = new Ball(_owner, new Vec2(), 0);
                ball2 = null;
            }

            line = new Line(_owner, new Vec2(), new Vec2());

            //position the colliders
            Update(offset, rotation);
        }

        public void addToManager(ColliderManager manager, bool isTrigger = false)
        {
            manager.AddSolidCollider(line);
            manager.AddSolidCollider(ball1);
            if (ball2 != null)
                manager.AddSolidCollider(ball2);
        }

        public void RemoveFromManager(ColliderManager manager, bool isTrigger = false)
        {
            manager.RemoveSolidCollider(line);
            manager.RemoveSolidCollider(ball1);
            if (ball2 != null)
                manager.RemoveSolidCollider(ball2);
        }

        /// <summary>
        /// Update the position with the new transform of our parent
        /// </summary>
        /// <param name="position">new offset</param>
        /// <param name="rotation">new rotation</param>
        public void Update(Vec2 position, float rotation)
        {
            Vec2 absoluteStart = start.Copy();
            absoluteStart.RotateDegrees(rotation);
            absoluteStart += position;

            Vec2 absoluteEnd = end.Copy();
            absoluteEnd.RotateDegrees(rotation);
            absoluteEnd += position;

            line.start = absoluteStart;
            line.end = absoluteEnd;
            ball1.position = absoluteStart;
            if (ball2 != null)
                ball2.position = absoluteEnd;
        }

        /// <summary>
        /// Get all colliders of this segment
        /// </summary>
        /// <returns>List of all colliders</returns>
        public List<Collider> GetColliders()
        {
            List<Collider> colliders = new List<Collider>();
            colliders.Add(line);
            colliders.Add(ball1);
            if (ball2 != null)
                colliders.Add(ball2);
            return colliders;
        }

        public override string ToString()
        {
            return("{" + start.ToString() + ", " + end.ToString() + "}");
        }
    }
}
