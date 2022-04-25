using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Physics;
using Visuals;
using System.Windows.Forms;

namespace UIElements
{
    public class ColliderPreview : EasyDraw
    {
        MultiSegmentCollider collider;
        MultiSegmentVisual visual;
        Pivot colliderPivot;
        public ColliderListPreview preview;

        public ColliderPreview(int width, int height, ColliderLoader loader, string name) : base(width, height)
        {
            Clear(System.Drawing.Color.DarkGreen);
            colliderPivot = new Pivot();
            AddChild(colliderPivot);
            collider = loader.GetCollider(name, colliderPivot);
            if(collider != null)
            {
                visual = new MultiSegmentVisual(collider);
                colliderPivot.AddChild(visual);

                Console.WriteLine("Collider " + name + " loaded");

                Vec2 center;
                Vec2 size;
                collider.GetBounds(out center, out size);
                float maxSize = Mathf.Min(width, height);
                colliderPivot.scale = maxSize / Mathf.Max(size.x, size.y);
                colliderPivot.position = (-center + size / 2) * colliderPivot.scale;
            }

            Button b = new Button(width, 30, System.Drawing.Color.White, System.Drawing.Color.LightGray);
            b.SetText(name);
            AddChild(b);
            b.SetXY(0, height - b.height);

            b.OnClick += () =>
            {
                if(ShowDialog("Delete " + name + " from the list?", "Delete Collider"))
                {
                    preview._colliderLoader.RemoveCollider(name);
                    preview.RegeneratePreview();
                }
            };
        }

        private bool ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
            System.Windows.Forms.Button confirmation = new System.Windows.Forms.Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            System.Windows.Forms.Button cancel = new System.Windows.Forms.Button() { Text = "Cancel", Left = 50, Width = 100, Top = 70, DialogResult = DialogResult.Cancel };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK;
        }
    }
}
