using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Layers
{
    class Layer : Pivot
    {
        public float paralaxX = 1;
        public float paralaxY = 1;
        public int index;
        Scene parentScene;

        /// <summary>
        /// Default constructor of a layer
        /// </summary>
        /// <param name="parentScene">
        /// The scene this layer is hooked to
        /// </param>
        /// <param name="paralaxX">
        /// Horizontal paralax effect strength
        /// </param>
        /// <param name="paralaxY">
        /// Vertical paralax effect strength
        /// </param>
        public Layer(Scene parentScene, float paralaxX = 1, float paralaxY = 1, int index = 0)
        {
            this.parentScene = parentScene;
            this.paralaxX = paralaxX;
            this.paralaxY = paralaxY;
            this.index = index;
        }

        /// <summary>
        /// Calculate the paralax offset every frame
        /// </summary>
        public void Update()
        {
            this.y = parentScene.y * (paralaxY - 1) * (1 / parentScene.scaleY);
            this.x = parentScene.x * (paralaxX - 1) * (1 / parentScene.scaleX);
        }
    }
}