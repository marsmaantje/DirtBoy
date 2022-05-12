using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

public class Cursor : Sprite
{
    public Cursor() : base("sprites/cursor.png")
    {
        SetOrigin(width / 2f, height / 2f);
    }

    public void Update()
    {
        position = Input.mousePosition;
        if (((MyGame)game).currentScene.player != null && !GlobalVariables.shooting)
        {
            visible = false;
        }
        else
            visible = true;
    }
}
