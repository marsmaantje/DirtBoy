using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Physics;
using Visuals;

namespace GameElements
{
    /// <summary>
    /// A single block, a type int in the constructor specifies which side to collapse into a triangle for angled collisions
    /// </summary>
    class Block : Pivot
    {
        /// <summary>
        /// Amount of variations on the default block, keep at zero for just the block
        /// </summary>
        public static readonly int VariationCount = 3;

        EasyDraw visual;
        readonly Vec2 pos0;
        readonly Vec2 pos1;
        readonly Vec2 pos2;
        readonly Vec2 pos3;
        readonly LineCollider top;
        readonly LineCollider right;
        readonly LineCollider bottom;
        readonly LineCollider left;
        readonly int variationIndex;

        public int health = 1;
        public readonly int maxHealth = 50;

        /// <summary>
        /// Get and set the global position of the block
        /// </summary>
        public Vec2 globalPosition
        {
            get => new Vec2(TransformPoint(0, 0));
            set => SetXY(new Vec2(InverseTransformPoint(value.x, value.y)));
        }

        /// <summary>
        /// Get and set the global position of the block
        /// </summary>
        public Vec2 position
        {
            get => new Vec2(x, y);
            set => SetXY(value.x, value.y);
        }
        Vec2 prevPosition;



        public Block(int width, int height, int variationIndex, int health)
        {
            this.health = health;

            this.variationIndex = variationIndex;

            //setup variables for the block based on the variationIndex
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;
            switch (variationIndex)
            {
                case 0: //block
                    pos0 = new Vec2(-halfWidth, -halfHeight);
                    pos1 = new Vec2(halfWidth, -halfHeight);
                    pos2 = new Vec2(halfWidth, halfHeight);
                    pos3 = new Vec2(-halfWidth, halfHeight);
                    break;
                case 1: //bottom right triangle
                    pos0 = new Vec2(-halfWidth, -halfHeight);
                    pos1 = new Vec2(halfWidth, -halfHeight);
                    pos2 = new Vec2(halfWidth, -halfHeight);
                    pos3 = new Vec2(-halfWidth, halfHeight);
                    break;
                case 2: //bottom left triangle
                    pos0 = new Vec2(-halfWidth, -halfHeight);
                    pos1 = new Vec2(halfWidth, -halfHeight);
                    pos2 = new Vec2(halfWidth, halfHeight);
                    pos3 = new Vec2(-halfWidth, -halfHeight);
                    break;
                default:
                    throw new NotImplementedException("No variation with this index");
                    break;
            }

            //create, parent and hide colliders
            top = new LineCollider(pos0, pos1);
            right = new LineCollider(pos1, pos2);
            bottom = new LineCollider(pos2, pos3);
            left = new LineCollider(pos3, pos0);

            AddChild(top);
            AddChild(right);
            AddChild(left);
            AddChild(bottom);
            top.visible = false;
            right.visible = false;
            left.visible = false;
            bottom.visible = false;

            //create visual
            visual = new EasyDraw(width, height, false);
            AddChild(visual);
            visual.SetOrigin(halfWidth, halfHeight);
            visual.SetXY(0, 0);
            updateVisuals();
        }

        public void Update()
        {
            if (globalPosition != prevPosition)
            {
                updateColliders();
            }

            prevPosition = globalPosition;

            updateVisuals();
        }

        /// <summary>
        /// Updates the position of all colliders
        /// </summary>
        void updateColliders()
        {
            Vec2 positionOffset = globalPosition;
            top.setStartEnd(positionOffset + pos0, positionOffset + pos1);
            right.setStartEnd(positionOffset + pos1, positionOffset + pos2);
            bottom.setStartEnd(positionOffset + pos2, positionOffset + pos3);
            left.setStartEnd(positionOffset + pos3, positionOffset + pos0);
        }

        /// <summary>
        /// Redraws the visuals fo the block
        /// </summary>
        /// <exception cref="NotImplementedException">When variationIndex is out of bounds</exception>
        void updateVisuals()
        {
            visual.NoStroke();
            visual.Fill(255, (int)Mathf.Clamp(Mathf.Map(health, 0, maxHealth, 255, -255), 0, 255), (int)Mathf.Clamp(Mathf.Map(health, 0, maxHealth, -255, 255), 0, 255));//lerp from purple to yellow based on health
            visual.Quad(pos0.x + visual.width / 2, pos0.y + visual.height / 2,
                pos1.x + visual.width / 2, pos1.y + visual.height / 2,
                pos2.x + visual.width / 2, pos2.y + visual.height / 2,
                pos3.x + visual.width / 2, pos3.y + visual.height / 2);
            
            visual.Fill(0);
            visual.TextAlign(CenterMode.Center, CenterMode.Min);
            switch(variationIndex)
            {
                case 0:
                    visual.TextAlign(CenterMode.Center, CenterMode.Center);
                    visual.Text(health.ToString(), visual.width / 2, visual.height / 2);
                    break;
                case 1:
                    visual.TextAlign(CenterMode.Min, CenterMode.Min);
                    visual.Text(health.ToString(), 0, 0);
                    break;
                case 2:
                    visual.TextAlign(CenterMode.Max, CenterMode.Min);
                    visual.Text(health.ToString(), visual.width, 0);
                    break;
                default:
                    throw new NotImplementedException("No variation with this index");
                    break;
            }
        }

        /// <summary>
        /// Removes one hitPoint, if dead, removes the block and all colliders
        /// </summary>
        public void Hit()
        {
            health--;
            if(health <= 0)
            {
                LateDestroy();
                top.LateDestroy();
                right.LateDestroy();
                bottom.LateDestroy();
                left.LateDestroy();

            }
        }
    }
}