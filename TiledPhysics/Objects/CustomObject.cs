using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
using Physics;

namespace Objects
{
    /// <summary>
    /// Base class for custom objects, allows for respawning,
    /// initializing and stores a reference to the current Scene and tiled object
    /// </summary>
    public class CustomObject : AnimationSprite
    {
        public TiledObject obj;
        protected Scene parentScene;

        public CustomObject(TiledObject obj, String filename, int cols, int rows, int frames = -1, bool keepInCache = false, bool addCollider = true) : base(filename, cols, rows, frames, keepInCache, addCollider)
        {
            this.obj = obj;
        }

        public CustomObject() : base("sprites/empty.png", 1, 1, addCollider: false) { }

        public virtual void initialize(Scene parentScene)
        {
            this.parentScene = parentScene;
        }

        public virtual void respawn() {}

        public virtual void OnHit(Mover source, Collider collider) { }
    }
}