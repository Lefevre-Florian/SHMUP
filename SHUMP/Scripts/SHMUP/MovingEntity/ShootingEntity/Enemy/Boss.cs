using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.Structure;
using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {
	
    public class Boss : ChargingEnemy
    {
        private static Boss instance;
        [Export] private int secondPhaseStart;
        [Export] private int thirdPhaseStart;

        [Export] private NodePath secondPhaseGraphics = default;
        [Export] private NodePath secondPhaseWeapons = default;
        [Export] private float droneRadius = 0f;
        [Export] private float droneSpeed = 0f;
        [Export] private float droneShootDelay = 1.5f;

        private const string BATTLE_DRONE_PATH = "res://Scenes/Prefab/Enemies/BattleDrone.tscn";
        private PackedScene droneScene;

        private const float YMARGIN = 60f;

        private Vector2 up;
        private Vector2 down;

        private List<Position2D> weapons = new List<Position2D>();

        private Action phase;

        private Boss ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            instance = this;

            base._Ready();

            up = new Vector2(forcedSpeed, Vector2.Up.y * (speed));
            down = new Vector2(forcedSpeed, Vector2.Down.y * (speed));
            velocity = up;

            droneScene = GD.Load<PackedScene>(BATTLE_DRONE_PATH);

            TriggerFirstPhase();
        }

        public static Boss GetInstance()
        {
            if (instance == null) instance = new Boss();
            return instance;
        }

        private void TriggerFirstPhase()
        {
            phase = TriggerFirstPhase;

            ChargeProcess();

            weapons.Add(canon);
        }

        private void TriggerSecondPhase()
        {
            phase = TriggerSecondPhase;

            GetNode<Node2D>(secondPhaseGraphics).Visible = true;

            foreach (Position2D lCanon in GetNode<Node2D>(secondPhaseWeapons).GetChildren())
                weapons.Add(lCanon);

            BattleDrone lDrone = droneScene.Instance<BattleDrone>();
            AddChild(lDrone);
            lDrone.Init(droneRadius, droneSpeed, droneShootDelay);
        }

        private void TriggerThridPhase()
        {
            phase = TriggerThridPhase;
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
            if (GlobalPosition.y <= YMARGIN)
                velocity = down;
            if (GlobalPosition.y >= screenSize.y - YMARGIN)
                velocity = up;
        }

        protected override void Shoot()
        {
            EnemyBullet lBullet;
            foreach (Position2D lCanon in weapons)
            {
                lBullet = bulletScene.Instance<EnemyBullet>();
                lBullet.Position = new Vector2(lCanon.GlobalPosition.x, lCanon.GlobalPosition.y);
                bulletContainer.AddChild(lBullet);
            }
        }

        public override void TakeDamage(int pDamage)
        {
            base.TakeDamage(pDamage);

            if(healthpoint <= secondPhaseStart && healthpoint > thirdPhaseStart 
            && phase == TriggerFirstPhase)
            {
                TriggerSecondPhase();
            }

            if(healthpoint <= thirdPhaseStart && phase == TriggerSecondPhase)
            {
              
            }

        }

        protected override void ChargeProcess()
        {
            if(phase == TriggerFirstPhase)
            {
                base.ChargeProcess();
            }
            
            velocity = up;
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}