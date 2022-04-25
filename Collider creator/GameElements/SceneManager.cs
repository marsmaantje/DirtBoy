using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Physics;
using GXPEngine;

namespace GameElements
{
	/// <summary>
	/// Class to manage the breakOut scene
	/// Has functionality to add lines, display score and shoot balls at blocks
	/// </summary>
	class SceneManager : GameObject
	{
		public static SceneManager main
		{
			get
			{
				if (_main == null)
				{
					_main = new SceneManager();

				}
				return _main;
			}
		}

		static SceneManager _main;

		List<Row> rows;
		List<Bullet> bullets;
		bool firstBullet = true;
		Vec2 shooterPosition;

		public SceneManager()
		{
			rows = new List<Row>();
			game.AddChild(this);
			bullets = new List<Bullet>();
			shooterPosition = new Vec2(game.width / 2, game.height - 20);
		}

        /// <summary>
		/// Adds a row to the scene and move the others down
		/// </summary>
		public void spawnRow()
		{
			foreach (Row row in rows)
			{
				row.y += 60;
			}

			Row newRow = new Row(8, game.width - 200, 20);
			AddChild(newRow);
			newRow.SetXY(game.width / 2, 50);
			rows.Add(newRow);
		}

        /// <summary>
		/// add a shooter to the scene
		/// </summary>
		/// <param name="position">Position to spawn the shooter at</param>
		public void spawnShooter(Vec2 position)
		{
			Shooter s = new Shooter(50, 5);
			s.globalPosition = position;
			game.AddChild(s);
		}

        /// <summary>
		/// When this object gets destroyed and it is the main object, set the main object to null so that it can be recreated
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this == _main)
				_main = null;
		}

        /// <summary>
		/// add the bullet to the list
		/// </summary>
		/// <param name="bullet"Bullet to add></param>
		public void addBullet(Bullet bullet)
		{
			bullets.Add(bullet);
		}

		/// <summary>
		/// Remove the bullet from the list.
		/// If it is the last bullet, spawn the next row and reset the shooter
		/// </summary>
		/// <param name="bullet"></param>
		public void removeBullet(Bullet bullet)
		{
			bullets.Remove(bullet);
			if (firstBullet)
			{
				firstBullet = false;
			}
			if (bullets.Count == 0)
			{
				shooterPosition = new Vec2(bullet.globalPosition.x, game.height - 20);
				spawnShooter(shooterPosition);
				spawnRow();
				firstBullet = true;
			}
		}
	}
}
