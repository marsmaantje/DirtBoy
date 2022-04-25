using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLoud;
using System.IO;

namespace Sound
{
    public class SoundSystem
    {
        static SoundSystem _main;
        public static SoundSystem Main
        {
            get
            {
                if (_main == null)
                    _main = new SoundSystem();
                return _main;
            }
        }

        Soloud SoundObject;
        WavStream sound;

        public SoundSystem()
        {
            //makes sure the soundsystem deïnitalizes when the process exits
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            SoundObject = new Soloud();
            SoundObject.init();
        }

        public void Play(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            switch(fi.Extension)
            {
                case ".ogg":
                    Console.WriteLine("ogg");
                    sound = new WavStream();
                    sound.load(filename);
                    SoundObject.play(sound);
                    break;
                case ".wav":
                    Console.WriteLine("wav");
                    break;
                default:
                    Console.WriteLine("unknown");
                    break;
            }
        }


        /// <summary>
        /// DeInitialize the sound object
        /// </summary>
        public void DeInit()
        {
            SoundObject.deinit();
        }

        /// <summary>
        /// DeInitialize the soundsystem when the process exits
        /// </summary>
        void OnProcessExit(object sender, EventArgs e)
        {
            DeInit();
        }
    }
}
