using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;

namespace Com.IsartDigital.SHMUP.GameEntities {

	public class Weapon : Node2D
	{

		[Export] private float shootFrequency = 1f;

		[Export] private NodePath pathCanon = default;
		[Export] private NodePath pathUpgrade = default;

		private const string PATH_BULLET = "res://Scenes/Prefab/Bullets/Bullet.tscn";
		private const string PATH_BULLET_CONTAINER = "../../BulletContainer";


		private bool canShoot = true;
		private Timer timer;

		private Position2D canon;
		private Node bulletContainer;
		private List<Position2D> canonPositions = new List<Position2D>();

		private Action doAction;

		public override void _Ready()
		{
			canon = GetNode<Position2D>(pathCanon);

			timer = new Timer();
			AddChild(timer);

			timer.WaitTime = shootFrequency;
			timer.Connect(EventTimer.TIMEOUT, this, nameof(Reset));

			bulletContainer = GetNode<Node>(PATH_BULLET_CONTAINER);

			Node2D lUpgradeContainer = GetNode<Node2D>(pathUpgrade);
            foreach (Polygon2D lItem in lUpgradeContainer.GetChildren())
            {
				canonPositions.Add(lItem.GetChild<Position2D>(0));
            }

			doAction = FirstPhaseShoot;
		}

		public void Shoot()
        {
            if (canShoot)
            {
				canShoot = false;

				timer.Start();
				timer.OneShot = true;

				doAction();
			}
        }

		private void FirstPhaseShoot()
        {
			PlayerBullet lBullet = GD.Load<PackedScene>(PATH_BULLET).Instance<PlayerBullet>();
			bulletContainer.AddChild(lBullet);
			lBullet.Position = canon.GlobalPosition;
		}

		private void SecondPhaseShoot()
        {
			FirstPhaseShoot();
			PlayerBullet lBullet;
			foreach (Position2D lPosition in canonPositions)
			{
				lBullet = GD.Load<PackedScene>(PATH_BULLET).Instance<PlayerBullet>();
				lBullet.Rotation = lPosition.GlobalRotation;
				lBullet.Position = lPosition.GlobalPosition;
				bulletContainer.AddChild(lBullet);
			}
		}

		private void ThirdPhaseShoot()
        {
			SecondPhaseShoot();
			// Other actions
        }

		private void Reset()
        {
			timer.Stop();
			canShoot = true;
        }

		public void UpgradeWeapon()
        {

			if (doAction == FirstPhaseShoot)
				doAction = SecondPhaseShoot;
			else if (doAction == SecondPhaseShoot)
				doAction = ThirdPhaseShoot;
			else
				return;

        }

	}

}