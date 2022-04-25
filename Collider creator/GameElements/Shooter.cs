using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Visuals;
using Physics;

namespace GameElements
{
    class Shooter : Pivot
    {
        #region position

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

        #endregion

        //amount of bullets to shoot
        int bulletCount;

        //are we shooting currently
        bool isShooting = false;

        //Time of last shot
        int intervalCounter = 0;

        //time between shots in frames
        int shootInterval;

        //direction we are shooting in
        Vec2 shootDirection = new Vec2(0,-1);

        float bulletSpeed;

        public Shooter(int BulletCount, float bulletSpeed, int shootInterval = 5)
        {
            this.bulletCount = BulletCount;
            this.bulletSpeed = bulletSpeed;
            this.shootInterval = shootInterval;
        }

        public void Update()
        {
            if (isShooting)
            {
                if (bulletCount > 0)
                    TryShot();
                else
                    LateDestroy();
            }
            else
            {
                UpdateAngle();
                if (Input.GetMouseButtonUp(0))
                    isShooting = true;
            }

            DrawShot();
        }

        /// <summary>
        /// Updates the shootDirection based on realtive position to the mouse
        /// </summary>
        void UpdateAngle()
        {
            Vec2 pos = globalPosition;
            Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY);
            shootDirection = (mousePos - pos).Normalized();
        }

        /// <summary>
        /// Draws a visual of the current angle we will shoot at
        /// </summary>
        void DrawShot()
        {
            Vec2 pos = globalPosition;
            Gizmos.DrawArrow(pos.x, pos.y, shootDirection.x * bulletSpeed * 20, shootDirection.y * bulletSpeed * 20);
        }

        /// <summary>
        /// Will shoot if the time between shots has elapsed and decrement the bulletCount
        /// </summary>
        void TryShot()
        {
            intervalCounter++;
            if(intervalCounter >= shootInterval)
            {
                intervalCounter = 0;
                Bullet b = new Bullet(globalPosition, 10, shootDirection * bulletSpeed);
                game.AddChild(b);
                bulletCount--;
                SceneManager.main.addBullet(b);
            }
        }
    }
}
