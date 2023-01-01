using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class BattleDrone : Drone
	{
        private const string PATH_LASER_SCENE = "res://Scenes/Prefab/Bullets/EnemyLaser.tscn";

        private float shootDelay;

        private Timer timer = null;
        private PackedScene laserScene = null;

        public override void _Ready()
        {
            laserScene = GD.Load<PackedScene>(PATH_LASER_SCENE);
        }

        public void Init(float pRadius, float pSpeed ,float pShootDelay,  bool pRotating = false)
        {
            base.Init(pRadius, pSpeed, pRotating);

            shootDelay = pShootDelay;

            timer = new Timer();
            timer.WaitTime = pShootDelay;
            AddChild(timer);
            timer.Start();

            timer.Connect(EventTimer.TIMEOUT, this, nameof(Shoot));
        }

        private void Shoot()
        {
            EnemyLaser lLaser = laserScene.Instance<EnemyLaser>();
            AddChild(lLaser);
            timer.WaitTime = shootDelay + lLaser.drawingDuration + lLaser.attackDuration;
        }

        protected override void Destructor()
        {
            Disconnect(EventTimer.TIMEOUT, this, nameof(Shoot));
            base.Destructor();
        }

    }

}