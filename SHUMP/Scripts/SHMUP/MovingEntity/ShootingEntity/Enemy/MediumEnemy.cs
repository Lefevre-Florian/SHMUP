using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.GameEntities;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class MediumEnemy : Enemy
	{

		[Export] private NodePath dronePath = default;
		[Export] private float droneRadius = 10f;
		[Export] private float droneSpeed = 100f;

		private float droneAngle = 0f;

		private Area2D drone;

		private bool isImmune = true;

		public override void _Ready()
		{
			base._Ready();

			drone = GetNode<Area2D>(dronePath);
			drone.Connect(EventArea2D.AREA_ENTERED, this, nameof(OnDroneCollisionEnter));
			drone.GlobalPosition = GlobalPosition + new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * droneRadius;

			velocity = Vector2.Left;
		}

		protected override void Shoot()
        {
			EnemyBullet lBullet = bulletScene.Instance<EnemyBullet>();
            lBullet.Position = canon.GlobalPosition;
            bulletContainer.AddChild(lBullet);
        }

        public override void TakeDamage(int pDamage)
        {
			if(!isImmune)
				base.TakeDamage(pDamage);
        }

		private void OnDroneCollisionEnter(Area2D pBody)
        {
			if(pBody is SPStrikeZone)
				DroneDestroy();
        }

		private void DroneDestroy()
        {
			drone.Disconnect(EventArea2D.AREA_ENTERED, this, nameof(OnDroneCollisionEnter));

			isImmune = false;
			drone.QueueFree();
			drone = null;
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
			if(drone != null)
            {
				droneAngle += delta;
				drone.GlobalPosition = GlobalPosition + new Vector2(Mathf.Cos(droneAngle * droneSpeed), Mathf.Sin(droneAngle * droneSpeed)) * droneRadius;
			}	
		}

    }

}