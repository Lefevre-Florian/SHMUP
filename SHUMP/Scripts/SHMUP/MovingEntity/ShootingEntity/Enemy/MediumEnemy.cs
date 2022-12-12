using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class MediumEnemy : Enemy
	{

		[Export] private float radius = 10f;

		private float angle = 0f;

		private Vector2 pivotPoint = Vector2.Zero;


		public override void _Ready()
		{
			base._Ready();

			velocity = Vector2.Left;
		}

        public override void _Process(float pDelta)
        {
            base._Process(pDelta);
			GlobalPosition += pivotPoint * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
			angle++;
        }

		protected override void Shoot()
        {
			EnemyBullet lBullet = bulletScene.Instance<EnemyBullet>();
            lBullet.Position = canon.GlobalPosition;
            bulletContainer.AddChild(lBullet);
        }

    }

}