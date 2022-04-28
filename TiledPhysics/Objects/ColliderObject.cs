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
    public enum CollisionType { CONCRETE, DIRT, NULL }
    /// <summary>
    /// Base class for custom objects, allows for respawning,
    /// initializing and stores a reference to the current Scene and tiled object
    /// </summary>
    public class ColliderObject : CustomObject
    {
        MultiSegmentCollider _collider;
        ColliderManager _colliderManager;
        ColliderLoader _colliderLoader;
        public CollisionType _collisionType;

        public ColliderObject(string filename, int cols, int rows, TiledObject obj) : this(filename, cols, rows, obj, -1)
        { }
        
        public ColliderObject(string filename, int cols, int rows, TiledObject obj, int frames = -1, bool keepInCache = false, bool addCollider = true) : base(obj, filename, cols, rows, frames, keepInCache, addCollider)
        {
            this.obj = obj;
            _collisionType = (CollisionType)obj.GetIntProperty("type", (int)CollisionType.NULL);
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
            if (_collider != null)
                _collider.AddToManager(_colliderManager);
            else
                throw new Exception("Collider with name " + obj.GetStringProperty("ColliderName") + " not found");

            //AddChild(new MultiSegmentVisual(_collider));
        }
        void OnCollision(GameObject other)
        {
            if (other is EasyDraw easyDraw)
            {
                if (easyDraw.parent is Player player)
                {
                    //Console.WriteLine("player got hit");
                }
            }
        }
    }
}