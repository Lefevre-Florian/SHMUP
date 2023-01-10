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

		private const string PATH_PARTICLES_CONTAINER = "../../ParticlesContainer";

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
				if(Enemy.enemies[i] != null)
					SetAnimation(pLightningScene, Enemy.enemies[i]);
            }		

			lLength = PopcornEnemy.popcornEnemies.Count;
			for (int i = lLength - 1; i >= 0; i--)
            {
				if (PopcornEnemy.popcornEnemies[i] != null)
					SetAnimation(pLightningScene, PopcornEnemy.popcornEnemies[i]);
            }
			
			GetTree().CreateTimer(lightningDuration).Connect(EventTimer.TIMEOUT, this, nameof(ScreenShake));
			GetTree().CreateTimer(lightningDuration + (blipDuration * nBlipAnimationEnd)).Connect(EventTimer.TIMEOUT, this, nameof(Destroy));
        }

		/// <summary>
		/// Start tweens animations and add lightning strike effect
		/// </summary>
		/// <param name="pLightningScene"></param>
		/// <param name="pEntity"></param>
		private void SetAnimation(PackedScene pLightningScene, MovingEntity pEntity)
        {
			if (pEntity == null)
				return;

			SceneTreeTween lTween = GetTree().CreateTween();

			Vector2 lInitialScale = pEntity.Scale;
			Color lInitialColor = pEntity.Modulate;

			Lightning lLightning = pLightningScene.Instance<Lightning>();
			particlesContainer.AddChild(lLightning);
			lLightning.DrawLightning(new Vector2(pEntity.GlobalPosition.x, pEntity.GlobalPosition.y - lightningSize),
									 pEntity.Position,
									 lightningDuration);

			lTween.Chain();
			lTween.TweenProperty(pEntity, PROPERTY_SCALE, new Vector2(lInitialScale.x + squashStrechForce, lInitialScale.y - squashStrechForce), animationDuration/2)
				  .SetTrans(animationTransition)
				  .SetEase(animationEaseType)
				  .SetDelay(lightningDuration);
			lTween.TweenProperty(pEntity, PROPERTY_SCALE, lInitialScale, animationDuration/2)
				  .SetTrans(animationTransition)
				  .SetEase(animationEaseType);

            for (int i = 0; i < nBlipAnimationEnd; i++)
            {
				lTween.TweenProperty(pEntity, PROPERTY_MODULATE, blipColor, blipDuration / (nBlipAnimationEnd+1) * i);
				lTween.TweenProperty(pEntity, PROPERTY_MODULATE, lInitialColor, blipDuration / (nBlipAnimationEnd+1) * i);
			}

			lTween.TweenCallback(this, nameof(DestroyEntity), new Godot.Collections.Array(pEntity));
			lTween.Play();
        }


		/// <summary>
		/// Destroy the entity individualy depending of is type
		/// </summary>
		/// <param name="pEntity"></param>
		private void DestroyEntity(MovingEntity pEntity)
        {
			if (pEntity == null)
				return;

			if (pEntity is Enemy)
            {
				if (pEntity is Boss)
					((Boss)pEntity).TakeDamage(damage);
				else
					((Enemy)pEntity).Destroy();
			}
            else pEntity.QueueFree();
        }

		private void Destroy()
        {
			GD.Print("Destroyed !");
		}

		private void ScreenShake()
        {
			Camera.GetInstance().SetActionShake();
        }

	}

}