using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

namespace Objects
{
    class CameraTrigger : CustomObject
    {
        public Pivot target;

        public CameraTrigger(TiledObject obj) : base(obj, "sprites/empty.png", -1, -1)
        {
            this.obj = obj;
        }

        public CameraTrigger(String filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows)
        {
            this.obj = obj;
        }

        /// <summary>
        /// need to create a seperate constructor for custom variables since the object is created by the tiledLoader but needs references to the scene it is connected to
        /// </summary>
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);

            this.collider.isTrigger = true;
            this.visible = false;

            createTarget();
        }

        /// <summary>
        /// Creates the pivot as a child of the current object with the correct x and y scale for the camera to encompass the entire area defined in tiled
        /// </summary>
        void createTarget()
        {
            target = new Pivot();
            AddChild(target);
            float targetX = (float)obj.Width / (float)game.width;
            float targetY = (float)obj.Height / (float)game.height;
            float targetSize = Mathf.Max(targetX, targetY);
            Vector2 actualSize = InverseTransformDirection(targetSize, targetSize);
            actualSize = parentScene.TransformDirection(actualSize.x, actualSize.y);
            target.SetScaleXY(actualSize.x, actualSize.y); //MATH!
        }
    }
}
