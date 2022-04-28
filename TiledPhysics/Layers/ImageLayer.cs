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
            bool repeating = obj.repeatX;
            SpriteBatch batch = new SpriteBatch();
            int spriteCount = repeating ? 40 : 1;

            for (int i = 0; i < spriteCount; i++)
            {
                Sprite image = new Sprite(Path.Combine(loader.FolderName, obj.Image.FileName), false, false);
                image.x = obj.offsetX + (image.width * (i - Mathf.Ceiling(spriteCount/2)));
                image.y = obj.offsetY;
                image.alpha = obj.Opacity;
                batch.AddChild(image);
            }
            batch.Freeze();
            AddChild(batch);
        }
    }
}
