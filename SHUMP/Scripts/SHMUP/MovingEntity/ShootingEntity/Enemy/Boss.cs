using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
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
        [Export] private float droneRespawnDelay = 2f;

        [Export] private NodePath thirdPhaseGraphics = default;
        [Export] private NodePath thirdPhaseWeapons = default;
        [Export] private float helperSpeed;
        [Export] private float helperThrowingDelay;
        [Export] private int nhelperThrowingEntity = 3;

        private const string BATTLE_DRONE_PATH = "res://Scenes/Prefab/Enemies/BattleDrone.tscn";
        private PackedScene droneScene;

        private const float YMARGIN = 60f;

        //Replace by boss size
        private const float XMARGIN = 80f;

        private const string BOOMERANG_PATH = "res://Scenes/Prefab/Bullets/EnemyBoomerang.tscn";
        private PackedScene boomerangScene;

        private Node2D thrower;
        private Position2D throwerWeapon;
        private Timer throwerTimer = new Timer();

        private Vector2 up;
        private Vector2 down;

        private List<Position2D> weapons = new List<Position2D>();

        private Action phase;

        private BattleDrone drone = null;

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

            InitChargeProcess();

            weapons.Add(canon);
        }

        private void TriggerSecondPhase()
        {
            phase = TriggerSecondPhase;

            GetNode<Node2D>(secondPhaseGraphics).Visible = true;

            StopChargeProcess();

            foreach (Position2D lCanon in GetNode<Node2D>(secondPhaseWeapons).GetChildren())
                weapons.Add(lCanon);

            AddDrone();
        }

        private void TriggerThridPhase()
        {
            phase = TriggerThridPhase;

            thrower = GetNode<Node2D>(thirdPhaseGraphics);
            throwerWeapon = GetNode<Position2D>(thirdPhaseWeapons);

            thrower.Visible = true;

            boomerangScene = GD.Load<PackedScene>(BOOMERANG_PATH);

            AddChild(throwerTimer);
            throwerTimer.WaitTime = helperThrowingDelay;
            throwerTimer.Connect(EventTimer.TIMEOUT, this, nameof(ThrowerShoot));
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
            if (GlobalPosition.y <= YMARGIN)
                velocity = down;
            if (GlobalPosition.y >= screenSize.y - YMARGIN)
                velocity = up;

            if(phase == TriggerThridPhase)
            {
                GD.Print(thrower.GlobalPosition);
                if (thrower.GlobalPosition.x <= XMARGIN)
                    thrower.GlobalPosition += Vector2.Right * helperSpeed * delta;
                if (thrower.GlobalPosition.x >= screenSize.x - XMARGIN)
                    thrower.GlobalPosition += Vector2.Left * helperSpeed * delta;
            }
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

        private void ThrowerShoot()
        {
            EnemyBoomerang lBoomerang = boomerangScene.Instance<EnemyBoomerang>();

            bulletContainer.AddChild(lBoomerang);
            lBoomerang.Position = throwerWeapon.GlobalPosition;
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
                TriggerThridPhase();
            }

        }

        protected override void StartChargeProcess()
        {
            if(phase == TriggerFirstPhase)
            {
                base.StartChargeProcess();
            }
            
            velocity = up;
        }

        private void AddDrone()
        {
            drone = droneScene.Instance<BattleDrone>();
            AddChild(drone);
            drone.Init(droneRadius, droneSpeed, droneShootDelay);
            drone.Connect(nameof(BattleDrone.Destroyed), this, nameof(DroneDestroyed));
        }

        private void DroneDestroyed()
        {
            drone.Disconnect(nameof(BattleDrone.Destroyed), this, nameof(DroneDestroyed));
            drone = null;

            if(phase == TriggerSecondPhase || phase == TriggerThridPhase)
                GetTree().CreateTimer(droneRespawnDelay).Connect(EventTimer.TIMEOUT, this, nameof(AddDrone));
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}