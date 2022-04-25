using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.IO;

namespace Layers
{
    /// <summary>
    /// special layer for images, supports repeating the texture if required
    /// </summary>
    class ImageLayer : Layer
    {

        public ImageLayer(TiledMapParser.TiledLoader loader, TiledMapParser.ImageLayer obj, Scene parentScene, float paralaxX = 1, float paralaxY = 1) : base(parentScene, paralaxX, paralaxY)
        {
            //duplicate the image a couple times if it should be repeated
            bool repeating = obj.GetBoolProperty("Repeating", false);

            for (int i = 0; i < (repeating ? 10 : 1); i++)
            {
                Sprite image = new Sprite(Path.Combine(loader.FolderName, obj.Image.FileName), false, false);
                image.x = obj.offsetX + (image.width * i);
                image.y = obj.offsetY;
                image.alpha = obj.Opacity;

                AddChild(image);
            }   

        }
    }
}
