using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;

namespace Com.IsartDigital.SHMUP.GameEntities {

	public class Weapon : Node2D
	{

		[Export] private float shootFrequency = 1f;

		private const string PATH_CANON = "Position2D";
		private Position2D canon;

		private bool canShoot = true;
		private Timer timer;

		private const string PATH_BULLET = "res://Scenes/Prefab/Bullet.tscn";
		private const string PATH_BULLET_CONTAINER = "../../BulletContainer";

		private Node bulletContainer;

		public override void _Ready()
		{
			canon = GetNode<Position2D>(PATH_CANON);

			timer = new Timer();
			AddChild(timer);

			timer.WaitTime = shootFrequency;
			timer.Connect(EventTimer.TIMEOUT, this, nameof(Reset));

			bulletContainer = GetNode<Node>(PATH_BULLET_CONTAINER);
		}

		public void Shoot()
        {
            if (canShoot)
            {
				canShoot = false;

				timer.Start();
				timer.OneShot = true;

				PlayerBullet lBullet = GD.Load<PackedScene>(PATH_BULLET).Instance<PlayerBullet>();
				bulletContainer.AddChild(lBullet);
				lBullet.GlobalPosition = canon.GlobalPosition;
			}
        }

		private void Reset()
        {
			timer.Stop();
			canShoot = true;
        }

	}

}