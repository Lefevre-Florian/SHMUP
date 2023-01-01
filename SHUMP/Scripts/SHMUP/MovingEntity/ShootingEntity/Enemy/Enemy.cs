using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.GameEntities.StaticEntities;
using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.SHMUP.GameEntities;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public abstract class Enemy : ShootingEntity
	{

		public static List<Enemy> enemies = new List<Enemy>();

		[Export] private float shootDelay = 1f;
		[Export] private int bodyDamage = 1;
		[Export] private uint score = 100;

		[Export] private NodePath weaponPath;

		[Export] private PackedScene drop = null;

		private const string PATH_BULLET_PREFAB = "res://Scenes/Prefab/Bullets/EnemyBullet.tscn";
		private const string PATH_SCORE_POPUP = "res://Scenes/Prefab/Juiciness/FlyingScore.tscn";

		private const string PATH_BULLET_CONTAINER = "../../BulletContainer";
		private const string TRIGGER_TAG = "Trigger";

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

			bulletContainer = GetNode<Node>(PATH_BULLET_CONTAINER);
			canon = GetNode<Position2D>(weaponPath);

			bulletScene = GD.Load<PackedScene>(PATH_BULLET_PREFAB);

			timer = new Timer();
			timer.WaitTime = shootDelay;

			timer.Connect(EventTimer.TIMEOUT, this, nameof(Shoot));
			AddChild(timer);
		}

        protected override void SetActionMoveAndShoot()
        {
            base.SetActionMoveAndShoot();
			enemies.Add(this);
			if(timer.TimeLeft <= 0)
				timer.Start();
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
			if (GlobalPosition.x < 0)
				Destroy();
		}

        protected virtual void Shoot() { }

        protected override void OnAreaEnter(Area2D pBody)
        {
			if (pBody.IsInGroup(TRIGGER_TAG))
				SetActionMoveAndShoot();

			if (pBody is Player.Player)
				((Player.Player)pBody).TakeDamage(bodyDamage);
        }

        protected override void Destroy()
        {
			enemies.Remove(this);

			if(healthpoint <= 0)
            {
				FlyingScore lScore = GD.Load<PackedScene>(PATH_SCORE_POPUP).Instance<FlyingScore>();
				GetParent().AddChild(lScore);
				lScore.RectPosition = Position;
				lScore.SetScore(score);
			}

			if(drop != null && healthpoint <= 0)
            {
				Area2D lCollectible = drop.Instance<Area2D>();
				AddChild(lCollectible);
				lCollectible.Position = GlobalPosition;
            }

			base.Destroy();
        }

        protected override void Destructor()
        {
			if (timer.IsConnected(EventTimer.TIMEOUT, this, nameof(Shoot)))
				timer.Disconnect(EventTimer.TIMEOUT, this, nameof(Shoot));
			timer.QueueFree();
			base.Destructor();
        }

    }

}