using Godot;
using System;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.GameEntities;
using Com.IsartDigital.SHMUP.UI;
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

        #region Controls
        private const string MOVE_UP = "Up";
        private const string MOVE_DOWN = "Down";
        private const string MOVE_LEFT = "Left";
        private const string MOVE_RIGHT = "Right";

        private const string SHOT = "Shoot";
        private const string PAUSE = "Pause";
        private const string SPECIAL = "Special";
        private const string GOD_MODE = "God_mode";
        private const string SMART_BOMB = "Bomb";
        #endregion

        private float forcedSpeed;

        private const string SPECIALFEATURE_PATH = "res://Scenes/Prefab/SpecialFeature/SpecialFeature.tscn";
        private const string SMARTBOMB_PATH = "res://Scenes/Prefab/Bullets/SmartBomb.tscn";

        private const string PROPERTY_MODULATE = "modulate";

        private const int NB_SMARTBOMB = 2;
        
        public Weapon canon;

        private UIManager uiManager;
        private BackgroundManager backgroundManager;

        private PackedScene specialFeatureScene;

        public int nSmartBomb = NB_SMARTBOMB;
        private bool canSmartBomb = true;

        private bool invincibility = false;
        public bool specialFeature = false;

        private SceneTreeTimer specialFeatureDelaytimer;

        [Signal]
        public delegate void LifeState(int pLifePoint);

        [Signal]
        public delegate void SmarbombState(bool pState);

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

            velocity = new Vector2(forcedSpeed, 0);
            doAction = SetActionMove;
        }

        public override void _Input(InputEvent pEvent)
        {
            velocity = new Vector2(forcedSpeed, 0);

            if (!specialFeature)
            {
                if (Input.IsActionPressed(MOVE_UP) && GlobalPosition.y > 0)
                    velocity = new Vector2(forcedSpeed, -1 * (forcedSpeed + speed));
                else if (Input.IsActionPressed(MOVE_DOWN) && GlobalPosition.y < screenSize.y)
                    velocity = new Vector2(forcedSpeed, 1 * (forcedSpeed + speed));
                else if (Input.IsActionPressed(MOVE_LEFT) && GlobalPosition.x > 0)
                    velocity = new Vector2(-1 * (forcedSpeed + speed), 0);
                else if (Input.IsActionPressed(MOVE_RIGHT) && GlobalPosition.x < screenSize.x)
                    velocity = new Vector2(1 * (forcedSpeed + speed), 0);

                if (Input.IsActionJustPressed(SPECIAL))
                    EnableSpecialFeature();

            }

            if (Input.IsActionPressed(SHOT))
                canon.Shoot();

            if (Input.IsActionJustPressed(PAUSE))
                uiManager.CallPopup();

            if (Input.IsActionJustPressed(GOD_MODE))
                invincibility = !invincibility;

            if (Input.IsActionJustPressed(SMART_BOMB) && canSmartBomb)
                CallSmartbomb();

        }

        public static Player GetInstance()
        {
			if (instance == null)
				instance = new Player();
			return instance;
        }

        public override void TakeDamage(int pDamage)
        {
            if (invincibility == false)
            {
                base.TakeDamage(pDamage);
                EmitSignal(nameof(LifeState), healthpoint);

                if (healthpoint == 0)
                    uiManager.TriggerGameOver();

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
            if (healthpoint > maxHealthPoint)
                healthpoint = maxHealthPoint;
            EmitSignal(nameof(LifeState), healthpoint);
        }

        private void CallSmartbomb()
        {
            if (nSmartBomb <= 0)
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

            specialFeatureDelaytimer = GetTree().CreateTimer(lSP.duration + specialFeatureDelay);
            specialFeatureDelaytimer.Connect(EventTimer.TIMEOUT, this, nameof(DisableSpecialFeature));
        }

        private void DisableSpecialFeature()
        {
            specialFeature = false;
            specialFeatureDelaytimer.Disconnect(EventTimer.TIMEOUT, this, nameof(DisableSpecialFeature));
        }

        private void DisableSpecialFeature(SpecialFeature pSpecialFeature)
        {
            pSpecialFeature.Disconnect(nameof(SpecialFeature.Finished), this, nameof(DisableSpecialFeature));
            specialFeatureDelaytimer.TimeLeft = specialFeatureDelay;
        }

        public int GetHealthPoint()
        {
            return healthpoint;
        }

        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null)
				instance = null;
            base.Dispose(pDisposing);
        }

    }

}