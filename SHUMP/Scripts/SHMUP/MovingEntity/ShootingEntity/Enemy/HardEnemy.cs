using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.GameEntities.StaticEntities;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class HardEnemy : Enemy
	{
        [Export] private int nBullet = 3;
        [Export] private float moveDelay = 1f;
        [Export] private float openingAngle = 90f;
        [Export] private int maxLoop = 2;

        private const string PATH_MINE_BULLET = "res://Scenes/Prefab/Bullets/EnemyMine.tscn";
        private const string PATH_CANON_RENDERER = "Renderer/Weapon";
        
        private const float MARGIN = 50f;

        private float internalTime;

        private int phase = 0;

        private bool isStatic = true;
        private bool isCharging = false;

        private float forcedSpeed;

        private Timer timer;

        private Vector2 initialPosition;
        private PackedScene minePrefab;

        private Polygon2D canonRenderer;

        public override void _Ready()
		{
			base._Ready();

            forcedSpeed = BackgroundManager.GetInstance().forcedSpeed;
            internalTime = GlobalPosition.DistanceTo(new Vector2(MARGIN, GlobalPosition.y)) / (speed+forcedSpeed);

            // Replace by SceneTreeTimer;
            timer = new Timer();
            timer.OneShot = true;
            timer.WaitTime = moveDelay;
            timer.Connect(EventTimer.TIMEOUT, this, nameof(ChangePattern));
            AddChild(timer);
            timer.Start();

            velocity = new Vector2(forcedSpeed, 0);
            minePrefab = GD.Load<PackedScene>(PATH_MINE_BULLET);

            canonRenderer = GetNode<Polygon2D>(PATH_CANON_RENDERER);
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

        private void ChangePattern()
        {
            if (isStatic && !isCharging)
            {
                velocity = Vector2.Left * speed;

                phase++;

                if(maxLoop != phase)
                {
                    isCharging = true;
                    isStatic = false;

                    timer.WaitTime = internalTime;
                }

            }else if (!isStatic && isCharging)
            {
                EnemyMine lMine = minePrefab.Instance<EnemyMine>();
                lMine.Position = GlobalPosition;
                bulletContainer.AddChild(lMine);

                velocity = Vector2.Right * (speed + forcedSpeed*2);
                isCharging = false;
                isStatic = false;
                timer.WaitTime = internalTime;
            }
            else
            {
                velocity = new Vector2(forcedSpeed, 0);
                timer.WaitTime = moveDelay;

                isStatic = true;
            }
            timer.Start();
        }
    }

}