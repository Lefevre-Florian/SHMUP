using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.GameEntities.StaticEntities;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class HardEnemy : ChargingEnemy
	{
        [Export] private int nBullet = 3;
        [Export] private float openingAngle = 90f;
        [Export] private int maxLoop = 2;

        private const string PATH_MINE_BULLET = "res://Scenes/Prefab/Bullets/EnemyMine.tscn";

        private int phase = 0;

        private PackedScene minePrefab;

        public override void _Ready()
		{
			base._Ready();

            velocity = new Vector2(forcedSpeed, 0);
            minePrefab = GD.Load<PackedScene>(PATH_MINE_BULLET);

            ChargeProcess();
        }

        protected override void Shoot()
        {
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

        protected override void ChargeProcess()
        {
            if (phase <= maxLoop)
            {
                base.ChargeProcess();
                velocity = new Vector2(forcedSpeed, 0f);
            }
            else
            {
                velocity = Vector2.Left * speed;
            }
        }

    }

}