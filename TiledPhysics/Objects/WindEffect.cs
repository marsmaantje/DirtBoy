using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using GXPEngine;

namespace Objects
{
    class WindEffect : CustomObject
    {
        float startScale;
        
        public WindEffect(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows)
        { }
        
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            startScale = scaleX;
        }
        void Update()
        {
            _mirrorX = (Time.time / 1000f) % 1 < 0.5f;
            scaleX = startScale * (1 + 0.1f * Mathf.Sin(Time.time / 100f) + 0.15f * Mathf.Sin(Time.time / 150f) + 0.2f * Mathf.Sin(Time.time / 190f));
        }
    }
}
