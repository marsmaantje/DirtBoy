using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;

namespace UIElements
{
    public class Button : EasyDraw
    {
        Color borderColor;
        Color highlightColor;
        Color baseColor;
        int borderWidth;
        string text;
        bool isOver = false;
        public bool IsOver
        {
            get => isOver;
        }

        public event ButtonAction OnClick;
        
        
        public Button(int width, int height, Color baseColor, Color highlightColor, int borderWidth = 1) : base(width, height, false)
        {
            this.baseColor = baseColor;
            this.highlightColor = highlightColor;
            this.borderWidth = borderWidth;
            this.text = "";
            OnClick += () => { };

            borderColor = baseColor;
            Draw();

            ButtonManager.Main.AddButton(this);
        }

        public void SetText(string text)
        {
            this.text = text;
            TextSize(1);
            float textWidth = TextWidth(text);
            float desiredWidth = width - borderWidth * 2;
            float textHeight = TextHeight(text);
            float desiredHeight = height - borderWidth * 2;
            float scale = Math.Min(desiredWidth / textWidth, desiredHeight / textHeight);
            TextSize(scale);
            Draw();
        }

        void Draw()
        {
            Clear(0);
            NoFill();
            StrokeWeight(borderWidth);
            Stroke(borderColor);
            float quadWidth = width - Mathf.Ceiling(borderWidth / 2f);
            float quadHeight = height - Mathf.Ceiling(borderWidth / 2f);
            Quad(0, 0, quadWidth, 0, quadWidth, quadHeight, 0, quadHeight);
            Fill(255);
            NoStroke();
            TextAlign(CenterMode.Center, CenterMode.Center);
            Text(text, width / 2, height / 2);

        }

        public void Update()
        {
            Vec2 relativeMousePosition = InverseTransformPoint(Input.mousePosition);
            isOver = (relativeMousePosition.x > 0 && relativeMousePosition.x < width && relativeMousePosition.y > 0 && relativeMousePosition.y < height);
            borderColor = isOver ? highlightColor : baseColor;
            Draw();

            if (isOver && Input.GetMouseButtonDown(0))
                OnClick();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ButtonManager.Main.RemoveButton(this);
        }
    }
    public delegate void ButtonAction();
}
