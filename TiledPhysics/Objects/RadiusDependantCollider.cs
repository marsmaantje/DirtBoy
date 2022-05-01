using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;
using GXPEngine;
using TiledMapParser;

namespace Objects
{
    public class RadiusDependantCollider : ColliderObject
    {
        float minRadius;
        bool pActive = true;//true because parent object adds colliders to the manager by default
        bool isActive = true;
        
        public RadiusDependantCollider(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        { }

        public RadiusDependantCollider(TiledObject obj) : base(obj)
        { }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            minRadius = obj.GetFloatProperty("minRadius", 10f);
        }

        public void Update()
        {
            if(parentScene.player != null)
            {
                pActive = isActive;
                isActive = parentScene.player.radius >= minRadius;

                if (isActive != pActive)
                {//our state changed
                    if (isActive)
                        _collider.AddToManager(_colliderManager);
                    else
                        _collider.RemoveFromManager(_colliderManager);
                }
            }
        }
    }
}