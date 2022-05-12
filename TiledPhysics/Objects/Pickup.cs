using System;
using GXPEngine;
using TiledMapParser;
using Physics;

namespace Objects
{
    public class Pickup : Button
    {

        public Pickup(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        {

        }
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            OnPressing += PickedUp;
        }

        public void Update()
        {
            base.Update();
            Animate(1/60f * 12);
        }

        void PickedUp()
        {
            GlobalVariables.soulCounter++;
            Sound soundEffect = new Sound("Sound/Pick_up_Soul.mp3");
            soundEffect.Play();
            LateDestroy();
        }
    }
}
