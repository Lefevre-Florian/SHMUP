using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy;

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
            if(pBody is PopcornEnemy || pBody is BasicEnemy)
            {
                QueueFree();
            }
        }

    }

}