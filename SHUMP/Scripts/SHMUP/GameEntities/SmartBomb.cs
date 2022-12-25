using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;

namespace Com.IsartDigital.SHMUP.GameEntities {

	public class SmartBomb : Node
	{

		[Export] private int damage = 1;

		private Camera2D camera = null;

		public override void _Ready()
		{
			GameManager lGameManager = GameManager.GetInstance();

			camera = lGameManager.camera;

			DestroyEntities();
		}

		private void DestroyEntities()
        {
			int lLength = Enemy.enemies.Count;

			for (int i = lLength - 1; i >= 0; i--)
				Enemy.enemies[i].TakeDamage(damage);

			lLength = Bullet.bullets.Count;
			for (int a = lLength - 1; a >= 0; a--)
				Bullet.bullets[a].Clean();

			lLength = PopcornEnemy.popcornEnemies.Count;
			for (int i = lLength - 1; i >= 0; i--)
				PopcornEnemy.popcornEnemies[i].Destroy();

			Destroy();
        }

		private void Destroy()
        {
			QueueFree();
        }

	}

}