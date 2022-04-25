using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Layers
{
    /// <summary>
    /// Layer for Objects, special functionality can be added if needed
    /// </summary>
    class ObjectLayer : Layer
    {
        public ObjectLayer(Scene parentScene, float paralaxX = 1, float paralaxY = 1) : base(parentScene, paralaxX, paralaxY)
        {

        }
    }
}
