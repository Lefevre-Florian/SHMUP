using Godot;
using System;
using Com.IsartDigital.SHMUP.Environment;
using System.Collections.Generic;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.GameEntities.StaticEntities;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class HardEnemy : ChargingEnemy
	{
        [Export] private int nBullet = 3;
        [Export] private int maxLoop = 2;
        [Export] private float activationDelay = 0.5f;
        [Export] private NodePath particlesPath = default;
        [Export] private List<NodePath> wheelPaths = new List<NodePath>();

        private const string PATH_MINE_BULLET = "res://Scenes/Prefab/Bullets/EnemyMine.tscn";

        private int phase = 0;

        private PackedScene minePrefab;
        private Particles2D smokeParticle = null;

        private List<Wheel> wheels = new List<Wheel>();

        public override void _Ready()
		{
			base._Ready();

            velocity = Vector2.Left * Mathf.Abs(speed - forcedSpeed);
            minePrefab = GD.Load<PackedScene>(PATH_MINE_BULLET);
            smokeParticle = GetNode<Particles2D>(particlesPath);

            foreach (NodePath lPath in wheelPaths)
                wheels.Add(GetNode<Wheel>(lPath));
        }

        protected override void SetActionMoveAndShoot()
        {
            GetTree().CreateTimer(activationDelay).Connect(EventTimer.TIMEOUT, this, nameof(InitChargeProcess));
            smokeParticle.Emitting = true;

            foreach (Wheel lWheel in wheels)
                lWheel.SetActionSpin();
            base.SetActionMoveAndShoot();
        }

        protected override void InitChargeProcess()
        {
            velocity = new Vector2(forcedSpeed, 0);
            base.InitChargeProcess();
        }

        protected override void Shoot()
        {
            base.Shoot();
            EnemyBullet lBullet;

            for (int i = 1; i < nBullet+1; i++)
            {
                lBullet = bulletScene.Instance<EnemyBullet>();
                lBullet.Position = canon.GlobalPosition;
                
                bulletContainer.AddChild(lBullet);
            }
        }

        protected override void Returning()
        {
            EnemyMine lMine = minePrefab.Instance<EnemyMine>();
            lMine.Position = GlobalPosition;
            bulletContainer.AddChild(lMine);

            phase++;

            base.Returning();
        }

        protected override void StartChargeProcess()
        {
            if (phase <= maxLoop)
            {
                base.StartChargeProcess();
                velocity = new Vector2(forcedSpeed, 0f);
            }
            else
            {
                velocity = Vector2.Left * speed;
            }
        }

    }

}