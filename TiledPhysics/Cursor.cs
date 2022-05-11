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
        SetOrigin(width / 2, height / 2);
    }

    public void Update()
    {
        position = Input.mousePosition;
    }
}
