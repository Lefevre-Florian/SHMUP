using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class ThrowerHelperEnemy : Enemy
	{

		private const string BOOMERANG_PATH = "res://Scenes/Prefab/Bullets/EnemyBoomerang.tscn";
		private PackedScene boomerangScene;

        private const float XMARGIN = 80f;

        private int nEntity = 0;
        private int entityThrowed = 0;

        private Timer spacingTimer;

        [Signal]
        public delegate void Destroyed();

        public void Init(float pSpeed, float pThrowingDelay, int pNEntity)
        {
            boomerangScene = GD.Load<PackedScene>(BOOMERANG_PATH);

            speed = pSpeed;
            nEntity = pNEntity;

            velocity = Vector2.Right * (speed + forcedSpeed);
            GD.Print(velocity);

            spacingTimer = new Timer();
            AddChild(spacingTimer);
            spacingTimer.WaitTime = pThrowingDelay;
            spacingTimer.Connect(EventTimer.TIMEOUT, this, nameof(Shoot));

            SetActionMoveAndShoot();
        }

        protected override void DoActionMove()
        {
            if (GlobalPosition.x < XMARGIN)
            {
                velocity = Vector2.Right * (speed + forcedSpeed);
                GD.Print(velocity);
            }
            if (GlobalPosition.x > screenSize.x - XMARGIN)
                velocity = Vector2.Left * Mathf.Abs(speed - forcedSpeed);
            base.DoActionMove();
        }

        protected override void Shoot()
        {
            if(entityThrowed++ == nEntity)
            {
                spacingTimer.Stop();
                entityThrowed = 0;
                return;
            }
            spacingTimer.Start();
            EnemyBoomerang lBoomerang = boomerangScene.Instance<EnemyBoomerang>();

            bulletContainer.AddChild(lBoomerang);
            lBoomerang.Position = canon.GlobalPosition;
        }

        public override void Destroy()
        {
            EmitSignal(nameof(Destroyed));
            base.Destroy();
        }

    }

}