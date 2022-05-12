using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

namespace Objects
{
    public class Button : CustomObject
    {
        bool _isPressed;
        public bool IsPressed { get => _isPressed; }
        public PressedEvent OnPressed;
        public PressedEvent OnPressing;
        public PressedEvent OnReleased;
        


        public Button(string filename, int cols, int rows, TiledObject obj, bool visible = true) : base(obj, filename, cols, rows)
        {
            _isPressed = false;
            OnPressed += ()=>{ };
            OnPressing += () => { };
            OnReleased += () => { };
            collider.isTrigger = true;
            this.visible = visible;
        }

        public void Update()
        {
            bool pPressed = _isPressed;
            _isPressed = collider.HitTest(parentScene.player.Collider);

            if (_isPressed && !pPressed)
            {
                OnPressed();
            }
            else if (!_isPressed && pPressed)
            {
                OnReleased();
            }
            else if (_isPressed && pPressed)
            {
                OnPressing();
            }
        }

        public delegate void PressedEvent();
    }
}
