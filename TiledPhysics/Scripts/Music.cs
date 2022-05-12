using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

namespace Scripts
{

    class Music : Script
    {
        SoundChannel musicChannel;
        Sound song;

        string filename;

        public Music(TiledObject obj) : this("sprites/empty.png", 1, 1, obj)
        { }

        public Music(string filename, int cols, int rows, TiledObject obj)
        {
            this.obj = obj;
            this.visible = false;

            this.filename = obj.GetStringProperty("songName");

            Console.WriteLine("trying to play song" + this.filename);
            song = new Sound(this.filename, true, false);

            musicChannel = song.Play();
            musicChannel.Volume = 0.5f;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Console.WriteLine("I am being deleted, HELP");
            musicChannel.Stop();
        }
    }
}
