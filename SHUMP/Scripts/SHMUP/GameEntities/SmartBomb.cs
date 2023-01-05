using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.GameEntities.SmartBombUtilities;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.GameEntities {

	public class SmartBomb : Node
	{

		[Export] private int damage = 1;

		[Export] private float delay;

		[Export] private float lightningDuration;
		[Export] private float lightningSize;

		[Export] private float squashStrechForce;
		[Export] private float animationDuration;
		[Export] private Tween.TransitionType animationTransition = default;
		[Export] private Tween.EaseType animationEaseType = default;

		[Export] private int nBlipAnimationEnd;
		[Export] private float blipDuration;
		[Export] private Color blipColor = default;

		[Export] private float weakMotoroVibration;
		[Export] private float strongMotorVibration;
		[Export] private float vibrationDuration;

		private const string PROPERTY_SCALE = "scale";
		private const string PROPERTY_MODULATE = "modulate";

		private const string PATH_LIGHTNING_SCENE = "res://Scenes/Prefab/Juiciness/Particles/Lightning.tscn";

		private const string PATH_PARTICLES_CONTAINER = "../../particlesContainer";

		private Node2D particlesContainer = null;

		public override void _Ready()
		{
			PackedScene lLightningScene = GD.Load<PackedScene>(PATH_LIGHTNING_SCENE);

			particlesContainer = GetNode<Node2D>(PATH_PARTICLES_CONTAINER);

			VibrationManager.SetVibration(weakMotoroVibration, strongMotorVibration, vibrationDuration);
			DestroyEntities(lLightningScene);
		}

		private void DestroyEntities(PackedScene pLightningScene)
        {
			int lLength = Bullet.bullets.Count;
			for (int i = lLength - 1; i >= 0; i--)
				Bullet.bullets[i].QueueFree();

			lLength = Enemy.enemies.Count;
			for (int i = lLength - 1; i >= 0; i--)
            {
				GD.Print(Enemy.enemies[i]);
				SetAnimation(pLightningScene, Enemy.enemies[i], GetTree().CreateTween());
			}

			lLength = PopcornEnemy.popcornEnemies.Count;
			for (int i = lLength - 1; i >= 0; i--)
            {
				GD.Print(PopcornEnemy.popcornEnemies[i]);
				SetAnimation(pLightningScene, PopcornEnemy.popcornEnemies[i], GetTree().CreateTween());
			}

			GetTree().CreateTimer(lightningDuration).Connect(EventTimer.TIMEOUT, this, nameof(ScreenShake));
			QueueFree();
        }

		private void SetAnimation(PackedScene pLightningScene, MovingEntity pEntity, SceneTreeTween pTween)
        {
			Vector2 lInitialScale = pEntity.Scale;
			Color lInitialColor = pEntity.Modulate;

			Lightning lLightning = pLightningScene.Instance<Lightning>();
			particlesContainer.AddChild(lLightning);
			lLightning.DrawLightning(new Vector2(pEntity.GlobalPosition.x, pEntity.GlobalPosition.y - lightningSize),
									 pEntity.GlobalPosition,
									 lightningDuration);

			pTween.Chain();
			pTween.TweenProperty(pEntity, PROPERTY_SCALE, new Vector2(lInitialScale.x + squashStrechForce, lInitialScale.y - squashStrechForce), animationDuration/2)
				  .SetTrans(animationTransition)
				  .SetEase(animationEaseType)
				  .SetDelay(lightningDuration);
			pTween.TweenProperty(pEntity, PROPERTY_SCALE, lInitialScale, animationDuration/2)
				  .SetTrans(animationTransition)
				  .SetEase(animationEaseType);

			pTween.SetLoops(nBlipAnimationEnd);
			pTween.TweenProperty(pEntity, PROPERTY_MODULATE, blipColor, blipDuration / nBlipAnimationEnd);
			pTween.TweenProperty(pEntity, PROPERTY_MODULATE, lInitialColor, blipDuration / nBlipAnimationEnd);

			pTween.TweenCallback(this, nameof(DestroyEntity), new Godot.Collections.Array(pEntity));
			pTween.Play();
        }

		private void DestroyEntity(MovingEntity pEntity)
        {
			if (pEntity is Boss)
				((Boss)pEntity).TakeDamage(damage);
			else
				pEntity.QueueFree();
        }

		private void ScreenShake()
        {
			Camera.GetInstance().SetActionShake();
        }

	}

}