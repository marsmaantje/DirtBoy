using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Layers
{
    /// <summary>
    /// Layer for Tiles, special functionality can be added if needed
    /// </summary>
    class TileLayer : Layer
    {

        public TileLayer(Scene parentScene, float paralaxX = 1, float paralaxY = 1) : base(parentScene, paralaxX, paralaxY)
        {

        }
    }
}