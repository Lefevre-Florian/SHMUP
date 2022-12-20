using Godot;
using System;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.GameEntities;
using Com.IsartDigital.SHMUP.UI;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player {

	public class Player : ShootingEntity
	{

		private static Player instance;

        [Export] private float recoveryTime = 1f;
        [Export] private int nRecoveryBlip = 2;
        [Export] private Color recoveryColor;

        #region Controls
        private const string MOVE_UP = "Up";
        private const string MOVE_DOWN = "Down";
        private const string MOVE_LEFT = "Left";
        private const string MOVE_RIGHT = "Right";

        private const string SHOT = "Shoot";
        private const string PAUSE = "Pause";
        private const string SPECIAL = "Special";
        private const string GOD_MODE = "God_mode";
        #endregion

        private float forcedSpeed;

        private const string SPECIALFEATURE_PATH = "res://Scenes/Prefab/SpecialFeature/SpecialFeature.tscn";

        private const string WEAPON_PATH = "Weapon";
        private const string HUD_PATH = "../../../UI";

        private const string PROPERTY_MODULATE = "modulate";
        
        public Weapon canon;

        private UIManager uiManager;
        private BackgroundManager backgroundManager;

        private Hud hud;

        private PackedScene specialFeatureScene;

        private bool invincibility = false;
        public bool specialFeature = false;

        public delegate bool LifeState();

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

            canon = GetNode<Weapon>(WEAPON_PATH);

            hud = GetNode<Hud>(HUD_PATH);

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
                    SpecialFeature();

            }

            if (Input.IsActionPressed(SHOT))
                canon.Shoot();

            if (Input.IsActionJustPressed(PAUSE))
                uiManager.CallPopup();

            if (Input.IsActionPressed(GOD_MODE))
                invincibility = !invincibility;

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
                hud.UpdateLifeHUD(pDamage, true);

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
            hud.UpdateLifeHUD(pHealthPoint);
        }

        private void SpecialFeature()
        {
            backgroundManager.SlowMotion();
            SpecialFeature lSP = specialFeatureScene.Instance<SpecialFeature>();
            GetParent().AddChild(lSP);
            lSP.Init(this);
            specialFeature = true;
        }

        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null)
				instance = null;
            base.Dispose(pDisposing);
        }

    }

}