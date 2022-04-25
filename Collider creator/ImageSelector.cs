using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Windows.Forms;

public class ImageSelector : Pivot
{
    Sprite image;
    Vec2 dragPosition;
    Vec2 OldPosition;

    public ImageSelector()
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            image = new Sprite(dialog.FileName);
            AddChild(image);
            Vec2 imageCenter = new Vec2(image.width / 2, image.height / 2);
            Vec2 gameCenter = new Vec2(game.width / 2, game.height / 2);
            position = gameCenter - imageCenter;
        }
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            dragPosition = Input.mousePosition;
            OldPosition = position;
        }

        if(Input.GetMouseButton(1))
        {
            Vec2 relativePosition = OldPosition - dragPosition;
            position = Input.mousePosition + relativePosition;
        }

        if(Input.scrolled)
        {
            Vec2 relativeMousePosition = new Vec2(InverseTransformPoint(Input.mouseX, Input.mouseY));
            float oldScale = scale;
            float newScale = scale + Input.scrollWheelValue * 0.1f * scale;
            SetScaleXY(newScale);
            Move(relativeMousePosition * Input.scrollWheelValue * -0.1f * oldScale);
        }
    }
}
