using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
using Physics;
using Visuals;

namespace Objects
{
    /// <summary>
    /// Base class for custom objects, allows for respawning,
    /// initializing and stores a reference to the current Scene and tiled object
    /// </summary>
    public class ColliderObject : CustomObject
    {
        MultiSegmentCollider _collider;
        ColliderManager _colliderManager;
        ColliderLoader _colliderLoader;

        public ColliderObject(string filename, int cols, int rows, TiledObject obj) : this(filename, cols, rows, obj, -1)
        { }
        
        public ColliderObject(string filename, int cols, int rows, TiledObject obj, int frames = -1, bool keepInCache = false, bool addCollider = true) : base(obj, filename, cols, rows, frames, keepInCache, addCollider)
        {
            this.obj = obj;
        }

        public ColliderObject() : base(null, "sprites/empty.png", 1, 1, addCollider: false) { }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Move(-Origin());
            SetOrigin(0, 0);

            _colliderManager = ColliderManager.main;
            _colliderLoader = ColliderLoader.main;
            _collider = _colliderLoader.GetCollider(obj.GetStringProperty("ColliderName"), this);
            _collider.AddToManager(_colliderManager);

            //AddChild(new MultiSegmentVisual(_collider));
        }
    }
}