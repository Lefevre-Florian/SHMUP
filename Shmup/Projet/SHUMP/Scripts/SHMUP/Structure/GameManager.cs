using Com.IsartDigital.SHMUP.MovingEntities;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Structure {

	public class GameManager : Node
	{

		private static GameManager instance;

		private GameManager(): base() { }

		public override void _Ready()
		{
			if(instance != null)
            {
				QueueFree();
				return;
            }
			instance = this;

			// Clear list

			PopcornEnemy.popcornEnemies.Clear();
			Enemy.enemies.Clear();
			Bullet.bullets.Clear();

		}

		public static GameManager GetInstance()
        {
			if (instance == null) instance = new GameManager();
			return instance;
        }


        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null) instance = null;
            base.Dispose(pDisposing);
        }

    }

}