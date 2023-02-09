using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.SHMUP.GameEntities;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.Particles;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public abstract class Enemy : ShootingEntity
	{

		public static List<Enemy> enemies = new List<Enemy>();

		[Export] protected float shootDelay = 1f;
		[Export] private int bodyDamage = 1;
		[Export] private uint score = 100;

		[Export] private NodePath weaponPath;
		[Export] private PackedScene drop = null;

		[Export] private NodePath rendererPath = default;

		[Export] private float tweenDamageDuration;
		[Export] private Tween.TransitionType tweenDamageTransition = default;
		[Export] private Tween.EaseType tweenEaseType = default;
		[Export] private Color damageColor = default;

		[Export] protected AudioStreamOGGVorbis soundDeath = null;
		[Export] protected AudioStreamOGGVorbis soundShoot = null;

		private const string PATH_BULLET_PREFAB = "res://Scenes/Prefab/Bullets/EnemyBullet.tscn";
		private const string PATH_SCORE_POPUP = "res://Scenes/Prefab/Juiciness/FlyingScore.tscn";
		private const string PATH_EXPLOSION = "res://Scenes/Prefab/Juiciness/Particles/CarExplosion.tscn";

		private const string PATH_BULLET_CONTAINER = "../../BulletContainer";
		private const string PATH_COLLECTIBLE_CONTAINER = "../../CollecitbleContainer";
		private const string PATH_PARTICLES_CONTAINER = "../../ParticlesContainer";

		private const string TRIGGER_TAG = "Trigger";

		private const string PROPERTY_COLOR = "color";

		protected Node bulletContainer;
		private Node2D collectibleContainer;

		protected Position2D canon;

		protected PackedScene bulletScene;

		protected bool canShoot = true;

		private Timer timer = null;

		protected float forcedSpeed;

		private Polygon2D body = null;
		private Color initialColors = default;

		public override void _Ready()
		{
			base._Ready();
			forcedSpeed = BackgroundManager.GetInstance().forcedSpeed;

			body = GetNode<Polygon2D>(rendererPath);
			initialColors = body.Color;

			bulletContainer = GetNode<Node>(PATH_BULLET_CONTAINER);
			collectibleContainer = GetNode<Node2D>(PATH_COLLECTIBLE_CONTAINER);

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
				QueueFree();
		}

        protected virtual void Shoot() 
		{
			soundManager.GetAudioPlayer(soundShoot, this);
		}

        public override void TakeDamage(int pDamage)
        {
			SetAnimation();
            base.TakeDamage(pDamage);
        }

        protected override void OnAreaEnter(Area2D pBody)
        {
			if (pBody.IsInGroup(TRIGGER_TAG))
				SetActionMoveAndShoot();

			if (pBody is Player.Player)
				((Player.Player)pBody).TakeDamage(bodyDamage);
        }

		protected virtual void SetAnimation()
        {
			SceneTreeTween lTween;
			lTween = GetTree().CreateTween();
			lTween.Chain();
			lTween.TweenProperty(body, PROPERTY_COLOR, damageColor, tweenDamageDuration / 2)
				  .SetTrans(tweenDamageTransition)
				  .SetEase(tweenEaseType);
			lTween.TweenProperty(body, PROPERTY_COLOR, initialColors, tweenDamageDuration / 2)
				  .SetTrans(tweenDamageTransition)
				  .SetEase(tweenEaseType);
			lTween.Play();
		}

        public override void Destroy()
        {
			FlyingScore lScore = GD.Load<PackedScene>(PATH_SCORE_POPUP).Instance<FlyingScore>();
			GetParent().AddChild(lScore);
			lScore.RectPosition = Position;
			lScore.SetScore(score);

			if(drop != null)
            {
				Area2D lCollectible = drop.Instance<Area2D>();
				collectibleContainer.AddChild(lCollectible);
				lCollectible.Position = Position;
            }

			Node2D lParticlesContainer = GetNode<Node2D>(PATH_PARTICLES_CONTAINER);
			Explosion lExplosion = GD.Load<PackedScene>(PATH_EXPLOSION).Instance<Explosion>();
			lParticlesContainer.AddChild(lExplosion);
			lExplosion.Position = GlobalPosition;
			soundManager.GetAudioPlayer(soundDeath, lExplosion);

			QueueFree();
        }

        protected override void Destructor()
        {
			if(enemies.Contains(this))
				enemies.Remove(this);
			if (timer.IsConnected(EventTimer.TIMEOUT, this, nameof(Shoot)))
				timer.Disconnect(EventTimer.TIMEOUT, this, nameof(Shoot));
			timer.QueueFree();
			base.Destructor();
        }

    }

}