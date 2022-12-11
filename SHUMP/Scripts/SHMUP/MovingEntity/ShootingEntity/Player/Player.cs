using Godot;
using System;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.GameEntities;
using Com.IsartDigital.SHMUP.UI;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player {

	public class Player : ShootingEntity
	{

		private static Player instance;

        [Export] private float revoceryTime = 1f;
        [Export] private Color recoveryColor;

        #region Controls
        private const string MOVE_UP = "Up";
        private const string MOVE_DOWN = "Down";
        private const string MOVE_LEFT = "Left";
        private const string MOVE_RIGHT = "Right";

        private const string SHOT = "Shoot";
        private const string PAUSE = "Pause";
        private const string GOD_MODE = "God_mode";
        #endregion

        private float forcedSpeed;

        private const string WEAPON_PATH = "Weapon";
        private const string HUD_PATH = "../../../UI";
        
        private Weapon canon;
        private UIManager uiManager;
        private Hud hud;

        private bool invincibility = false;

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

            forcedSpeed = BackgroundManager.GetInstance().forcedSpeed;

            uiManager = UIManager.GetInstance();

            canon = GetNode<Weapon>(WEAPON_PATH);

            hud = GetNode<Hud>(HUD_PATH);
		}

        public override void _Input(InputEvent pEvent)
        {
            if (Input.IsActionPressed(MOVE_UP) && GlobalPosition.y > 0)
                velocity = new Vector2(forcedSpeed, -1 * (forcedSpeed + speed));
            else if (Input.IsActionPressed(MOVE_DOWN) && GlobalPosition.y < screenSize.y)
                velocity = new Vector2(forcedSpeed, 1 * (forcedSpeed + speed));
            else if (Input.IsActionPressed(MOVE_LEFT) && GlobalPosition.x > 0)
                velocity = new Vector2(-1 *(forcedSpeed + speed), 0);
            else if (Input.IsActionPressed(MOVE_RIGHT) && GlobalPosition.x < screenSize.x)
                velocity = new Vector2( 1 * (forcedSpeed + speed), 0);
            else
                velocity = new Vector2(forcedSpeed ,0);

            if (Input.IsActionJustPressed(SHOT))
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
            }
        }

        public void RegainHealth(int pHealthPoint)
        {
            healthpoint += pHealthPoint;
            hud.UpdateLifeHUD(pHealthPoint);
        }

        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null)
				instance = null;
            base.Dispose(pDisposing);
        }

    }

}