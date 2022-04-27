using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Physics;
using TiledMapParser;

namespace Objects
{
    public class HealthEffect : CustomObject
    {
        float health;
        public HealthEffect(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider: true)
        {
            collider.isTrigger = true;
            switch (obj.GetIntProperty("type"))
            {
                case 1: health = 0.25f; Console.WriteLine("puddle of dirt was created"); break;
                case 2: health = -0.25f; Console.WriteLine("puddle of water was created"); break;
                case 3: health = -10f; Console.WriteLine("a river was created"); break;
                default: Console.WriteLine("add " + obj.GetIntProperty("type") +  " to the list"); break;
            }
        }

        public void Update()
        {

        }
    }
}
