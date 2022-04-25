using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Visuals
{
    class BallVisual : EasyDraw
    {
        public BallVisual(float radius) : base ((int)radius*2 + 1, (int)radius*2 + 1)
        {
            SetOrigin(radius, radius);
            Ellipse(radius, radius, 2 * radius, 2 * radius);
        }
    }
}
