using Godot;
using System;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.GameEntities;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player {

	public class Player : ShootingEntity
	{

		private static Player instance;

        #region Controls
        private const string MOVE_UP = "Up";
        private const string MOVE_DOWN = "Down";
        private const string MOVE_LEFT = "Left";
        private const string MOVE_RIGHT = "Right";

        private const string SHOT = "Shoot";
        private const string PAUSE = "Pause";
        #endregion

        private float forcedSpeed;

        private const string WEAPON_PATH = "Weapon";
        private Weapon canon;

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

            canon = GetNode<Weapon>(WEAPON_PATH);
		}

        public override void _Input(InputEvent pEvent)
        {
            if (Input.IsActionPressed(MOVE_UP))
                velocity = Vector2.Up * speed;
            else if (Input.IsActionPressed(MOVE_DOWN))
                velocity = Vector2.Down * speed;
            else if (Input.IsActionPressed(MOVE_LEFT))
                velocity = Vector2.Left * speed;
            else if (Input.IsActionPressed(MOVE_RIGHT))
                velocity = Vector2.Right * speed;
            else
                velocity = new Vector2(forcedSpeed ,0);

            if (Input.IsActionJustPressed(SHOT))
                canon.Shoot();

            if (Input.IsActionJustPressed(PAUSE))
                UIManager.GetInstance().CallPopup();

        }

        public static Player GetInstance()
        {
			if (instance == null)
				instance = new Player();
			return instance;
        }

        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null)
				instance = null;
            base.Dispose(pDisposing);
        }

    }

}