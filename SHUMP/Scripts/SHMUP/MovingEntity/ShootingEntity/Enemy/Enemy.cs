using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public abstract class Enemy : ShootingEntity
	{
		[Export] private float shootDelay = 1f;
		[Export] private int bodyDamage = 1;

		[Export] private NodePath weaponPath;

		private const string PATH_BULLET_PREFAB = "res://Scenes/Prefab/Bullets/EnemyBullet.tscn";
		private const string PATH_BULLET_CONTAINER = "../../BulletContainer";
		private const string PATH_CANON = "Renderer/Weapon/Position2D";

		private Player.Player target;

		protected Node bulletContainer;
		protected Position2D canon;

		protected PackedScene bulletScene;

		protected bool canShoot = true;

		private Timer timer;

		protected float forcedSpeed;

		public override void _Ready()
		{
			base._Ready();
			forcedSpeed = BackgroundManager.GetInstance().forcedSpeed;

			target = Player.Player.GetInstance();
			bulletContainer = GetNode<Node>(PATH_BULLET_CONTAINER);
			canon = GetNode<Position2D>(weaponPath);

			bulletScene = GD.Load<PackedScene>(PATH_BULLET_PREFAB);

			timer = new Timer();
			timer.WaitTime = shootDelay;

			timer.Connect(EventTimer.TIMEOUT, this, nameof(Shoot));
			AddChild(timer);

			SetActionMoveAndShoot();
		}

        protected override void SetActionMoveAndShoot()
        {
            base.SetActionMoveAndShoot();
			if(timer.TimeLeft <= 0)
				timer.Start();
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
			if (GlobalPosition.x < 0)
				QueueFree();
		}

        protected virtual void Shoot() { }

        protected override void OnAreaEnter(Area2D pBody)
        {
			if (pBody is Player.Player)
				((Player.Player)pBody).TakeDamage(bodyDamage);
        }

    }

}