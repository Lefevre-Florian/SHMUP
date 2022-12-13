using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities {

	public class EnemyMine : Area2D
	{

		[Export] private int nBullet = 5;
		[Export] private float explosionRadius = 15f;
		//Juiciness
		[Export] private Color colorSignal = default;
		[Export] private int nRepetition;
		[Export] private float explosionDelay;

		private const string PATH_MODULATE = "modulate";
		private const string PATH_BULLET = "res://Scenes/Prefab/Bullets/EnemyBullet.tscn";

		private PackedScene bulletPrefab;

		private Color initColor;

		public override void _Ready()
		{
			initColor = Modulate;
			float lTimeBetween = explosionDelay / nRepetition;

			SceneTreeTween lTween = CreateTween();
			lTween.Chain();
            for (int i = 0; i < nRepetition; i++)
            {
				lTween.TweenProperty(this, PATH_MODULATE, colorSignal, lTimeBetween);
				lTween.TweenProperty(this, PATH_MODULATE, initColor, lTimeBetween);
			}

			bulletPrefab = GD.Load<PackedScene>(PATH_BULLET);

			// Replace by SceneTreeTimer
			Timer lTimer = new Timer();
			lTimer.OneShot = true;
			lTimer.WaitTime = explosionDelay;
			AddChild(lTimer);
			lTimer.Connect(EventTimer.TIMEOUT, this, nameof(Explosion), new Godot.Collections.Array(lTimer));
			lTimer.Start();
		}

		private void Explosion(Timer pTimer)
        {
			pTimer.Disconnect(EventTimer.TIMEOUT, this, nameof(Explosion));
			pTimer.QueueFree();

			float lSpacing = (2 * Mathf.Pi) / nBullet;
			
			EnemyBullet lBullet;
			for (int i = 0; i < nBullet; i++)
            {
				lBullet = bulletPrefab.Instance<EnemyBullet>();
				lBullet.Position = GlobalPosition + new Vector2(Mathf.Cos(lSpacing * i), Mathf.Sin(lSpacing * i)) * explosionRadius;
				lBullet.Rotation = lSpacing * i;
				GetParent().AddChild(lBullet);
			}

			QueueFree();


        }

	}

}