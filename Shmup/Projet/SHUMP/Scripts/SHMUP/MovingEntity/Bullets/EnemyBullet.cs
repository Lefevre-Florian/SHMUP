using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public class EnemyBullet : Bullet
	{

		public override void _Ready()
		{
			base._Ready();

			velocity = Vector2.Left.Rotated(GlobalRotation) * speed;
		}

        protected override void OnAreaEnter(Area2D pBody)
        {
            if(pBody is Player)
            {
                ((Player)pBody).TakeDamage(damage);
                QueueFree();
            }
        }

    }

}