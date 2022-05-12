using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

namespace UIElements
{
    class PlayButton : Button
    {
        /// <summary>
        /// fileNmae of the map this button should load when pressed
        /// </summary>
        string mapName;

        public PlayButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj) { }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            //get what level to load, otherwise get the current level so stuff doesnt break
            mapName = obj.GetStringProperty("mapName", "maps/Main Menu.tmx");
        }

        protected override void OnClicked()
        {
            Sound soundEffect = new Sound("Sound/Menu_Press.mp3");
            soundEffect.Play();
            ((MyGame)game).loadNewLevel(mapName);
        }


    }
}