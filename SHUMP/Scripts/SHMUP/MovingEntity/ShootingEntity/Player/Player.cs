using Godot;
using System;
using Com.IsartDigital.SHMUP.Structure;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player {

	public class Player : ShootingEntity
	{

		private static Player instance;

        #region Controls
        private const string MOVE_UP = "Up";
        private const string MOVE_DOWN = "Down";
        private const string MOVE_LEFT = "Left";
        private const string MOVE_RIGHT = "Right";
        #endregion

        private float forcedSpeed;
        private Vector2 direction;

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
		}

        public override void _Input(InputEvent pEvent)
        {
            if (Input.IsActionPressed(MOVE_UP))
                velocity = Vector2.Up * Mathf.Abs(forcedSpeed + speed);
            else if (Input.IsActionPressed(MOVE_DOWN))
                velocity = Vector2.Down * Mathf.Abs(forcedSpeed - speed);
            else if (Input.IsActionPressed(MOVE_LEFT))
                velocity = Vector2.Left * Mathf.Abs(forcedSpeed - speed);
            else if (Input.IsActionPressed(MOVE_RIGHT))
                velocity = Vector2.Right * Mathf.Abs(forcedSpeed - speed);
            else
                velocity = new Vector2(forcedSpeed ,0);
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