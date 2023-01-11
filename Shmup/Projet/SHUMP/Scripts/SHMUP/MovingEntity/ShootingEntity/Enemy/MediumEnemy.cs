using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class MediumEnemy : Enemy
	{

		[Export] private float inShootDelay;
		[Export] private int nEntity;
		[Export] private NodePath dronePath = default;
		[Export] private float droneRadius = 10f;
		[Export] private float droneSpeed = 100f;

		private float droneAngle = 0f;

		private int inBetweenCount = 0;

		private Drone drone;

		private bool isImmune = true;

		public override void _Ready()
		{
			base._Ready();
			velocity = Vector2.Left * Mathf.Abs(speed - forcedSpeed);
		}

        protected override void SetActionMoveAndShoot()
        {
			drone = GetNode<Drone>(dronePath);
			drone.Init(droneRadius, droneSpeed, true);


			drone.Connect(nameof(Drone.Destroyed), this, nameof(DestroyedDrone));
			base.SetActionMoveAndShoot();
		}

        protected override void Shoot()
        {
			EnemyBullet lBullet = bulletScene.Instance<EnemyBullet>();
            lBullet.Position = canon.GlobalPosition;
            bulletContainer.AddChild(lBullet);

			if (++inBetweenCount >= nEntity)
				inBetweenCount = 0;
			else
				GetTree().CreateTimer(inShootDelay).Connect(EventTimer.TIMEOUT, this, nameof(Shoot));
        }

        public override void TakeDamage(int pDamage)
        {
			if(!isImmune)
				base.TakeDamage(pDamage);
        }

        private void DestroyedDrone()
        {
			drone.Disconnect(nameof(Drone.Destroyed), this, nameof(DestroyedDrone));
			drone = null;
			isImmune = false;
        }

        public override void Destroy()
        {
			if (drone != null)
				DestroyedDrone();
            base.Destroy();
        }

    }

}