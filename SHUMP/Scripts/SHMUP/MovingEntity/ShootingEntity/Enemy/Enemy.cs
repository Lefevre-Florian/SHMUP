using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public abstract class Enemy : ShootingEntity
	{
		[Export] private float shootDelay = 1f;
		[Export] private int bodyDamage = 1;

		private const string PATH_BULLET_PREFAB = "res://Scenes/Prefab/Bullets/EnemyBullet.tscn";
		private const string PATH_BULLET_CONTAINER = "../../BulletContainer";
		private const string PATH_CANON = "Renderer/Weapon/Position2D";

		private Player.Player target;

		protected Node bulletContainer;
		protected Position2D canon;

		protected PackedScene bulletScene;

		protected bool canShoot = true;

		private Timer timer;


		public override void _Ready()
		{
			base._Ready();

			target = Player.Player.GetInstance();
			bulletContainer = GetNode<Node>(PATH_BULLET_CONTAINER);
			canon = GetNode<Position2D>(PATH_CANON);

			bulletScene = GD.Load<PackedScene>(PATH_BULLET_PREFAB);

			// Replace by SceneTreeTimer
			timer = new Timer();
			timer.WaitTime = shootDelay;

			timer.Connect(EventTimer.TIMEOUT, this, nameof(Shoot));
			AddChild(timer);

			doAction = SetActionMoveAndShoot;
		}

        protected override void SetActionMoveAndShoot()
        {
            base.SetActionMoveAndShoot();
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