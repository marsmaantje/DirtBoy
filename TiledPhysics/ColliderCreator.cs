using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Physics;
using GXPEngine;
using Visuals;
using System.Windows.Forms;

public class ColliderCreator : Pivot
{
    MultiSegmentCollider collider;
    ImageSelector image;
    MultiSegmentVisual visual;
    LineSegment mouseVisual;
    Vec2? startPoint;
    string colliderName = "";

    public ColliderCreator(ImageSelector image)
    {
        collider = new MultiSegmentCollider(image);
        visual = new MultiSegmentVisual(collider);
        this.image = image;
        mouseVisual = new LineSegment(new Vec2(), new Vec2(), pShowNormal: true);
        image.AddChild(visual);
        image.AddChild(this);
        image.AddChild(mouseVisual);
    }

    public void Update()
    {
        Vec2 relativeMousePosition = image.InverseTransformPoint(Input.mousePosition);
        if (startPoint.HasValue && (Input.GetKey(Key.LEFT_CTRL) || Input.GetKey(Key.RIGHT_CTRL)))
        {
            Vec2 positionDelta = relativeMousePosition - startPoint.Value;
            bool horizontal = Mathf.Abs(positionDelta.x) > Mathf.Abs(positionDelta.y);
            relativeMousePosition = new Vec2(horizontal ? relativeMousePosition.x : startPoint.Value.x, horizontal ? startPoint.Value.y : relativeMousePosition.y);
        }

        if (collider.segments != null && collider.segments.Count > 0)
        {//snap to first segment if close enough
            Vec2 globalMousePosition = Input.mousePosition;
            Vec2 globalStartPosition = image.TransformPoint(collider.segments[0].start);
            if (Vec2.Distance(globalMousePosition, globalStartPosition) < 20)
                relativeMousePosition = collider.segments[0].start;
        }

        if (Input.GetMouseButtonDown(0) && !UIElements.ButtonManager.Main.OverAny())
        {
            if (startPoint.HasValue)
                collider.AddSegment(startPoint.Value, relativeMousePosition);

            startPoint = relativeMousePosition;
            mouseVisual.start = startPoint.Value;
        }

        if (startPoint.HasValue)
            mouseVisual.end = relativeMousePosition;

        //If backspace is pressed, remove the last line / undo
        if (Input.GetKeyDown(Key.BACKSPACE))
        {
            if (visual.Segments.Count > 0)
            {
                startPoint = visual.Segments[visual.Segments.Count - 1].start;
                collider.RemoveLastSegment();
                mouseVisual.start = startPoint.Value;
            }
            else
            {
                startPoint = null;
                mouseVisual.start = new Vec2();
                mouseVisual.end = new Vec2();
            }
        }

        if (Input.GetKeyDown(Key.ENTER))
        {
            colliderName = new Prompt("Enter a name for the collider:", "Name").Result;
            Console.WriteLine(colliderName);
        }
    }

    /// <summary>
    /// Saves the current collider to the collider list
    /// </summary>
    public void Save()
    {
        ColliderLoader.main.AddCollider(colliderName, collider, true);
    }

    internal class Prompt : IDisposable
    {
        private Form prompt { get; set; }
        public string Result { get; }

        public Prompt(string text, string caption)
        {
            Result = ShowDialog(text, caption);
        }
        //use a using statement
        private string ShowDialog(string text, string caption)
        {
            prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        public void Dispose()
        {
            if (prompt != null)
            {
                prompt.Dispose();
            }
        }
    }
}