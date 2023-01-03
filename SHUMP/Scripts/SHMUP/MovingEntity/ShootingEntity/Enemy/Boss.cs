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

        [Export] private float helperSpeed;
        [Export] private float helperThrowingDelay;
        [Export] private int nhelperThrowingEntity = 3;
        [Export] private float helperThrowingRespawn;

        [Export] private float waitingDelay = 0.25f;

        private const string BATTLE_DRONE_PATH = "res://Scenes/Prefab/Enemies/BattleDrone.tscn";
        private PackedScene droneScene;

        private const float YMARGIN = 80f;

        //Replace by boss size
        private const float XMARGIN = 80f;

        private const string THROWER_PATH = "res://Scenes/Prefab/Enemies/Thrower.tscn";
        private PackedScene throwerScene;

        private ThrowerHelperEnemy thrower = null;

        private Vector2 up;
        private Vector2 down;

        private List<Position2D> weapons = new List<Position2D>();

        private Action phase;

        private bool invincibiliy = false;
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
            
            velocity = Vector2.Left * Mathf.Abs(speed - forcedSpeed);

            throwerScene = GD.Load<PackedScene>(THROWER_PATH);
            droneScene = GD.Load<PackedScene>(BATTLE_DRONE_PATH);
        }

        public static Boss GetInstance()
        {
            if (instance == null) instance = new Boss();
            return instance;
        }

        protected override void SetActionMoveAndShoot()
        {
            if (phase != null)
                return;

            base.SetActionMoveAndShoot();
            GetTree().CreateTimer(waitingDelay).Connect(EventTimer.TIMEOUT, this, nameof(TriggerFirstPhase));
        }

        private void TriggerFirstPhase()
        {
            phase = TriggerFirstPhase;
            velocity = up;

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

            AddThrower();
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
            if(!invincibiliy)
               base.TakeDamage(pDamage);

            if(healthpoint <= secondPhaseStart && healthpoint > thirdPhaseStart 
            && phase == TriggerFirstPhase)
                TriggerSecondPhase();


            if(healthpoint <= thirdPhaseStart && phase == TriggerSecondPhase)
                TriggerThridPhase();

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

            invincibiliy = true;
        }

        private void DroneDestroyed()
        {
            drone.Disconnect(nameof(BattleDrone.Destroyed), this, nameof(DroneDestroyed));
            drone = null;

            invincibiliy = false;

            if(phase == TriggerSecondPhase || phase == TriggerThridPhase)
                GetTree().CreateTimer(droneRespawnDelay).Connect(EventTimer.TIMEOUT, this, nameof(AddDrone));
        }

        private void AddThrower()
        {
            thrower = throwerScene.Instance<ThrowerHelperEnemy>();
            GetParent().AddChild(thrower);
            thrower.Init(helperSpeed, helperThrowingDelay, nhelperThrowingEntity);
            thrower.GlobalPosition = new Vector2(screenSize.x / 2, screenSize.y - YMARGIN);

            thrower.Connect(nameof(ThrowerHelperEnemy.Destroyed), this, nameof(ThrowerDestroyed));
        }

        private void ThrowerDestroyed()
        {
            thrower.Disconnect(nameof(ThrowerHelperEnemy.Destroyed), this, nameof(ThrowerDestroyed));
            thrower = null;

            GetTree().CreateTimer(helperThrowingRespawn).Connect(EventTimer.TIMEOUT, this, nameof(AddThrower));
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }

        protected override void Destroy()
        {
            UIManager.GetInstance().TriggerGameOver();
            if (thrower != null)
                thrower.QueueFree();
            base.Destroy();
        }
    }
}