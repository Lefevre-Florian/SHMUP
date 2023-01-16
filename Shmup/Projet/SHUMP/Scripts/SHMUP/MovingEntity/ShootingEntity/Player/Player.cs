using Godot;
using System;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.GameEntities;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player {

    public class Player : ShootingEntity
    {

        private static Player instance;

        [Export] private float recoveryTime = 1f;
        [Export] private int nRecoveryBlip = 2;
        [Export] private Color recoveryColor;

        [Export] public int maxSmartBomb = 5;
        [Export] public int maxHealthPoint = 3;

        [Export] private float specialFeatureDelay;

        [Export] private NodePath weaponPath = default;

        [Export] private AudioStreamOGGVorbis soundDeath = null;

        [Export] private NodePath firstRendererPath = default;
        [Export] private NodePath secondRendererPath = default;

        [Export] private NodePath smokePath = null;

        #region Controls
        private const string MOVE_UP = "Up";
        private const string MOVE_DOWN = "Down";
        private const string MOVE_LEFT = "Left";
        private const string MOVE_RIGHT = "Right";

        private const string SHOT = "Shoot";
        private const string SPECIAL = "Special";
        private const string GOD_MODE = "God_mode";
        private const string SMART_BOMB = "Bomb";

        private bool isShooting;

        private bool isMovingUp;
        private bool isMovingForward;
        private bool isMovingBackward;
        private bool isMovingDown;
        #endregion

        #region CONSTANT
        private const string SPECIALFEATURE_PATH = "res://Scenes/Prefab/SpecialFeature/SpecialFeature.tscn";
        private const string SMARTBOMB_PATH = "res://Scenes/Prefab/Bullets/SmartBomb.tscn"; 
        
        private const string PROPERTY_MODULATE = "modulate";
        
        private const int NB_SMARTBOMB = 2;

        private const float MARGINX = 50f;
        private const float MARGINY = 50f;
        private const float HUDSIZE = 100f;
        
        private float forcedSpeed;
        private const float REPULSION = 8;
        #endregion

        #region Renderer

        private Polygon2D fullLifeRenderer = null;
        private Polygon2D damagedRenderer = null;

        private Particles2D smoke = null;

        #endregion

        public Weapon canon;

        private UIManager uiManager;
        private BackgroundManager backgroundManager;

        private PackedScene specialFeatureScene;

        public int nSmartBomb = NB_SMARTBOMB;
        private bool canSmartBomb = true;

        private bool invincibility = false;
        private bool godMode = false;
        public bool specialFeature = false;

        private Timer specialFeatureDelaytimer = new Timer();

        private Vector2 direction = new Vector2();

        [Signal]
        public delegate void LifeState(int pLifePoint);

        [Signal]
        public delegate void SmarbombState(bool pState);

        [Signal]
        public delegate void SpecialFeatureState(float pDuration);

        private Player() : base() { }

		public override void _Ready()
		{
			if(instance != null)
            {
				QueueFree();
				return;
            }
			instance = this;

            base._Ready();

            backgroundManager = BackgroundManager.GetInstance();
            forcedSpeed = backgroundManager.forcedSpeed;

            uiManager = UIManager.GetInstance();

            canon = GetNode<Weapon>(weaponPath);

            specialFeatureScene = GD.Load<PackedScene>(SPECIALFEATURE_PATH);

            fullLifeRenderer = GetNode<Polygon2D>(firstRendererPath);
            damagedRenderer = GetNode<Polygon2D>(secondRendererPath);
            smoke = GetNode<Particles2D>(smokePath);

            AddChild(specialFeatureDelaytimer);

            velocity = new Vector2(forcedSpeed, 0);
            doAction = SetActionMove;
        }

        public override void _UnhandledInput(InputEvent pEvent)
        {
            direction = Vector2.Zero;

            if (!specialFeature)
            {
                isMovingUp = Input.IsActionPressed(MOVE_UP);
                isMovingDown = Input.IsActionPressed(MOVE_DOWN);
                isMovingForward = Input.IsActionPressed(MOVE_RIGHT);
                isMovingBackward = Input.IsActionPressed(MOVE_LEFT);
                
                isShooting = Input.IsActionPressed(SHOT);

                if (Input.IsActionJustPressed(SPECIAL) && specialFeatureDelaytimer.TimeLeft <= 0f)
                {
                    isMovingUp = false;
                    isMovingForward = false;
                    isMovingDown = false;
                    isMovingBackward = false;

                    EnableSpecialFeature();
                }
                    
            }

            if (Input.IsActionJustPressed(GOD_MODE))
                godMode = !godMode;

            if (Input.IsActionJustPressed(SMART_BOMB) && canSmartBomb)
                CallSmartbomb();

        }

        public static Player GetInstance()
        {
			if (instance == null)
				instance = new Player();
			return instance;
        }

        protected override void DoActionMove()
        {
            if (isShooting)
                canon.Shoot();

            direction = Vector2.Zero;
            if (isMovingUp && GlobalPosition.y > HUDSIZE)
                direction += new Vector2(forcedSpeed, -1 * (forcedSpeed + speed));
            if (isMovingDown && GlobalPosition.y < screenSize.y - MARGINY)
                direction += new Vector2(forcedSpeed, 1 * (forcedSpeed + speed));
            if (isMovingBackward && GlobalPosition.x > MARGINX)
                direction += new Vector2(-1 * (forcedSpeed + speed), 0);
            if (isMovingForward && GlobalPosition.x < screenSize.x - MARGINX)
                direction += new Vector2(1 * (forcedSpeed + speed), 0);

            if (direction == Vector2.Zero)
                direction = new Vector2(forcedSpeed, 0);
            velocity = direction;

            base.DoActionMove();
        }

        public override void TakeDamage(int pDamage)
        {
            if (invincibility == false && godMode == false)
            {
                base.TakeDamage(pDamage);
                EmitSignal(nameof(LifeState), healthpoint);

                if (healthpoint == 0)
                {
                    soundManager.GetAudioPlayer(soundDeath, GetParent());
                    uiManager.TriggerGameOver(false);
                }

                if (!damagedRenderer.Visible)
                {
                    damagedRenderer.Visible = true;
                    fullLifeRenderer.Visible = false;

                    smoke.Emitting = true;
                }

                invincibility = true;

                Color lSelfModulate = Modulate;
                SceneTreeTween lTween = CreateTween();
              
                lTween.Chain();
                for (int i = 0; i < nRecoveryBlip; i++)
                {
                    lTween.TweenProperty(this, PROPERTY_MODULATE, recoveryColor, recoveryTime/nRecoveryBlip);
                    lTween.TweenProperty(this, PROPERTY_MODULATE, lSelfModulate, recoveryTime/nRecoveryBlip);
                }
                lTween.TweenCallback(this, nameof(ResetDamaging), new Godot.Collections.Array(lTween));
            }
        }

        private void ResetDamaging(SceneTreeTween pTween)
        {
            invincibility = false;
        }

        public void RegainHealth(int pHealthPoint)
        {
            healthpoint += pHealthPoint;
            if (healthpoint >= maxHealthPoint)
            {
                healthpoint = maxHealthPoint;
                damagedRenderer.Visible = false;
                fullLifeRenderer.Visible = true;

                smoke.Emitting = false;
            }
            EmitSignal(nameof(LifeState), healthpoint);
        }

        private void CallSmartbomb()
        {
            if (nSmartBomb - 1 < 0)
                return;

            nSmartBomb--;

            SmartBomb lSmartBomb = GD.Load<PackedScene>(SMARTBOMB_PATH).Instance<SmartBomb>();
            AddChild(lSmartBomb);

            EmitSignal(nameof(SmarbombState), true);
        }

        public void RegainSmartBomb()
        {
            if (nSmartBomb+1 == maxSmartBomb)
                return;
            nSmartBomb++;
            EmitSignal(nameof(SmarbombState), false);
        }

        private void EnableSpecialFeature()
        {
            SpecialFeature lSP = specialFeatureScene.Instance<SpecialFeature>();
            GetParent().AddChild(lSP);
            lSP.Init(this);
            lSP.Connect(nameof(SpecialFeature.Finished), this, nameof(DisableSpecialFeature), new Godot.Collections.Array(lSP));
            specialFeature = true;

            specialFeatureDelaytimer.Connect(EventTimer.TIMEOUT, this, nameof(DisableSpecialFeature), new Godot.Collections.Array(lSP));
            specialFeatureDelaytimer.WaitTime = lSP.duration + specialFeatureDelay;
            specialFeatureDelaytimer.OneShot = true;
            specialFeatureDelaytimer.Start();

            EmitSignal(nameof(SpecialFeatureState), true, lSP.duration + specialFeatureDelay);
        }

        private void DisableSpecialFeature(SpecialFeature pSpecialFeature)
        {
            specialFeature = false;
            pSpecialFeature.Disconnect(nameof(SpecialFeature.Finished), this, nameof(DisableSpecialFeature));

            specialFeatureDelaytimer.Disconnect(EventTimer.TIMEOUT, this, nameof(DisableSpecialFeature));
            if (specialFeatureDelaytimer.TimeLeft > specialFeatureDelay)
            {
                specialFeatureDelaytimer.WaitTime = specialFeatureDelay;
                EmitSignal(nameof(SpecialFeatureState), false, specialFeatureDelay);
            }
            else
            {
                EmitSignal(nameof(SpecialFeatureState), false, specialFeatureDelaytimer.WaitTime);
            }
                
        }

        public int GetHealthPoint()
        {
            return healthpoint;
        }

        protected override void Destructor()
        {
            specialFeatureDelaytimer.Stop();
            if(specialFeatureDelaytimer.IsConnected(EventTimer.TIMEOUT, this, nameof(DisableSpecialFeature)))
                specialFeatureDelaytimer.Disconnect(EventTimer.TIMEOUT, this, nameof(DisableSpecialFeature));
            base.Destructor();
        }

        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null)
				instance = null;
            base.Dispose(pDisposing);
        }

    }

}