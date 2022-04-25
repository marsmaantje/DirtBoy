using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIElements
{
    class ButtonManager
    {
        static ButtonManager _main;
        public static ButtonManager Main
        {
            get
            {
                if (_main == null)
                {
                    _main = new ButtonManager();
                }
                return _main;
            }
        }

        List<Button> buttons;

        ButtonManager()
        {
            buttons = new List<Button>();
        }

        public void AddButton(Button button)
        {
            buttons.Add(button);
        }

        public void RemoveButton(Button button)
        {
            buttons.Remove(button);
        }
        
        public bool OverAny()
        {
            foreach (Button button in buttons)
            {
                if (button.IsOver)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
