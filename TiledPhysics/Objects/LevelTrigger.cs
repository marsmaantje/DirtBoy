using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using GXPEngine;

namespace Objects
{
    public class LevelTrigger : Button
    {
        string newMap;

        public LevelTrigger (string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj, false)
        {
            if(!obj.HasProperty("MapName", "string"))
                throw new Exception("no map name specified");
            newMap = obj.GetStringProperty("MapName");
            OnPressed += () => { ((MyGame)game).loadNewLevel(newMap); };
        }
    }
}
