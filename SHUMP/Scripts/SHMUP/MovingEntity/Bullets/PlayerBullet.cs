using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public class PlayerBullet : Bullet
	{

		public override void _Ready()
		{
			base._Ready();

			velocity = Vector2.Right.Rotated(Rotation) * speed;
		}

        protected override void OnAreaEnter(Area2D pBody)
        {
            if(pBody is Enemy)
            {
                ((ShootingEntity)pBody).TakeDamage(damage);
                Clean();
            }

            if(pBody is PopcornEnemy)
            {
                ((PopcornEnemy)pBody).Destroy();
                Clean();
            }
        }

    }

}