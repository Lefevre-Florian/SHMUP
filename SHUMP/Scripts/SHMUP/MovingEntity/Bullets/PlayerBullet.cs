using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public class PlayerBullet : Bullet
	{

		public override void _Ready()
		{
			base._Ready();

			velocity = Vector2.Right * speed;
		}

        protected override void OnAreaEnter(Area2D pBody)
        {
            // Enemy collision
        }

    }

}