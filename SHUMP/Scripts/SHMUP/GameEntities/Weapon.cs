using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;

namespace Com.IsartDigital.SHMUP.GameEntities {

	public class Weapon : Node2D
	{

		[Export] private float shootFrequency = 1f;

		[Export] private NodePath pathCanon = default;
		[Export] private NodePath pathFirstUpgrade = default;
		[Export] private NodePath pathSecondUpgrade = default;

		[Export] private AudioStreamOGGVorbis vfx = default;

		private const string PATH_BULLET = "res://Scenes/Prefab/Bullets/Bullet.tscn";
		private const string PATH_BULLET_CONTAINER = "../../BulletContainer";

		private bool canShoot = true;
		private Timer timer;

		private int phase = 0;

		private Position2D canon;
		private Node bulletContainer;
		private List<Position2D> canonPositions = new List<Position2D>();


		public override void _Ready()
		{
			timer = new Timer();
			AddChild(timer);

			timer.WaitTime = shootFrequency;
			timer.Connect(EventTimer.TIMEOUT, this, nameof(Reset));

			bulletContainer = GetNode<Node>(PATH_BULLET_CONTAINER);

			FirstPhaseShoot();
		}

		public void Shoot()
        {
            if (canShoot)
            {
				canShoot = false;

				timer.Start();
				timer.OneShot = true;

				ShootProcess();
			}
        }

		private void ShootProcess()
        {
			PlayerBullet lBullet;
			foreach (Position2D lPosition in canonPositions)
			{
				lBullet = GD.Load<PackedScene>(PATH_BULLET).Instance<PlayerBullet>();
				lBullet.Rotation = lPosition.GlobalRotation;
				lBullet.Position = lPosition.GlobalPosition;
				bulletContainer.AddChild(lBullet);
			}
		}

		private void FirstPhaseShoot()
        {
			phase++;

			Position2D lCanon = GetNode<Position2D>(pathCanon);
			canonPositions.Add(lCanon);
		}

		private void SecondPhaseShoot()
        {
			phase++;

			Node2D lUpgradeContainer = GetNode<Node2D>(pathFirstUpgrade);
			lUpgradeContainer.Visible = true;
			foreach (Polygon2D lItem in lUpgradeContainer.GetChildren())
			{
				canonPositions.Add(lItem.GetChild<Position2D>(0));
			}
		}

		private void ThirdPhaseShoot()
        {
			Node2D lUpgradeContainer = GetNode<Node2D>(pathSecondUpgrade);
			lUpgradeContainer.Visible = true;
			foreach (Polygon2D lItem in lUpgradeContainer.GetChildren())
			{
				canonPositions.Add(lItem.GetChild<Position2D>(0));
			}
		}

		private void Reset()
        {
			timer.Stop();
			canShoot = true;
        }

		public void UpgradeWeapon()
        {
			if (phase == 1)
				SecondPhaseShoot();
			else if (phase == 2)
				ThirdPhaseShoot();
			else
				return;
        }

	}

}