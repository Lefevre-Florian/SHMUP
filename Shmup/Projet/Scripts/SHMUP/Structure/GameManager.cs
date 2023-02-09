using Com.IsartDigital.SHMUP.MovingEntities;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Structure {

	public class GameManager : Node2D
	{

		private static GameManager instance;

		[Export] private int nFirework = 5;
		[Export] private float waitTime;
		[Export] private NodePath particlePath = null;
		[Export] private PackedScene fireworkFactory = default;
		[Export] private NodePath bulletContainerPath = default;

        private GameManager(): base() { }

		public override void _Ready()
		{
			if(instance != null)
            {
				QueueFree();
				return;
            }
			instance = this;

			GetTree().Paused = false;

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


		public void EndGame()
        {
			RandomNumberGenerator lRand = new RandomNumberGenerator();
			lRand.Randomize();

			Vector2 lScreenSize = GetViewportRect().Size;

			Particles2D lFirework = null;
			for (int i = 0; i < nFirework; i++)
			{
				lFirework = fireworkFactory.Instance<Particles2D>();
				GetNode<Node2D>(particlePath).AddChild(lFirework);
				lFirework.GlobalPosition = new Vector2(lRand.RandfRange(0f, lScreenSize.x), lRand.RandfRange(0f, lScreenSize.y));
				lFirework.Emitting = true;
			}
			GetTree().CreateTimer(lFirework.Lifetime).Connect(EventTimer.TIMEOUT, this, nameof(EndProcess));

		}

		private void EndProcess()
        {
			foreach (Bullet lBullet in GetNode<Node>(bulletContainerPath).GetChildren())
				lBullet.QueueFree();

			foreach (Particles2D lParticle in GetNode<Node2D>(particlePath).GetChildren())
				lParticle.QueueFree();
			UIManager.GetInstance().TriggerGameOver(true);
		}

        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null) instance = null;
            base.Dispose(pDisposing);
        }

    }

}