using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Physics;
using Visuals;

namespace UIElements
{
    public class ColliderPreview : EasyDraw
    {
        MultiSegmentCollider collider;
        MultiSegmentVisual visual;
        Pivot colliderPivot;

        public ColliderPreview(int width, int height, ColliderLoader loader, string name) : base(width, height)
        {
            Clear(System.Drawing.Color.DarkGreen);
            colliderPivot = new Pivot();
            AddChild(colliderPivot);
            collider = loader.GetCollider(name, colliderPivot);
            if(collider != null)
            {
                visual = new MultiSegmentVisual(collider);
                colliderPivot.AddChild(visual);

                Console.WriteLine("Collider " + name + " loaded");

                Vec2 center;
                Vec2 size;
                collider.GetBounds(out center, out size);
                float maxSize = Mathf.Min(width, height);
                colliderPivot.scale = maxSize / Mathf.Max(size.x, size.y);
                colliderPivot.position = (-center + size / 2) * colliderPivot.scale;
            }
        }
    }
}
