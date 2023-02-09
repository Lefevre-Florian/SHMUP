using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public class PlayerBullet : Bullet
	{

		public override void _Ready()
		{
			base._Ready();

            velocity = Vector2.Right.Rotated(Rotation) * (speed + Player.GetInstance().GetSpeed());
		}

        protected override void OnAreaEnter(Area2D pBody)
        {
            if(pBody is Enemy)
            {
                ((ShootingEntity)pBody).TakeDamage(damage);
                QueueFree();
            }

            if(pBody is PopcornEnemy)
            {
                ((PopcornEnemy)pBody).Destroy();
                QueueFree();
            }
        }

    }

}