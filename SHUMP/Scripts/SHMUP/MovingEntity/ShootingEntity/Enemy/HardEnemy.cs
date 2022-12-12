using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.Structure;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class HardEnemy : Enemy
	{
        [Export] private int nBullet = 3;
        [Export] private float bulletSpacing;

        private const float MARGIN = 60f;
        private float yPos;

        private bool patternComplete = false;
        private float forcedSpeed;

        private Vector2 up;
        private Vector2 down;

		public override void _Ready()
		{
			base._Ready();

            forcedSpeed = BackgroundManager.GetInstance().forcedSpeed;

            up = new Vector2(forcedSpeed, -1 * (forcedSpeed + speed));
            down = new Vector2(forcedSpeed, 1 * (forcedSpeed + speed));

            velocity = up;
            yPos = screenSize.y/2;
		}

        public override void _Process(float pDelta)
        {
            base._Process(pDelta);
            if (GlobalPosition.y <= MARGIN)
                velocity = down;
            if (GlobalPosition.y >= screenSize.y - MARGIN)
            {
                velocity = up;
                patternComplete = true;
            }

            if (patternComplete && GlobalPosition.y == yPos)
                velocity = Vector2.Left * speed;
        }

        protected override void Shoot()
        {
            EnemyBullet lBullet;
            Vector2 lPosition = canon.GlobalPosition;
            for (int i = 1; i < nBullet+1; i++)
            {
                lBullet = bulletScene.Instance<EnemyBullet>();
                lBullet.Position = new Vector2(lPosition.x + (bulletSpacing * i), lPosition.y);
                bulletContainer.AddChild(lBullet);
            }
        }
    }

}