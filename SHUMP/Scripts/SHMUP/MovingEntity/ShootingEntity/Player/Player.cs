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

        private Player() : base() { }

		public override void _Ready()
		{
			if(instance != null)
            {
				QueueFree();
				return;
            }
			instance = this;

            //forcedSpeed = BackgroundManager.GetInstance().forcedSpeed;
		}

        public override void _Input(InputEvent pEvent)
        {
            if (Input.IsActionPressed(MOVE_UP))
                direction = Vector2.Up;
            else if (Input.IsActionPressed(MOVE_DOWN))
                direction = Vector2.Down;
            else if (Input.IsActionPressed(MOVE_LEFT))
                direction = Vector2.Left;
            else if (Input.IsActionPressed(MOVE_RIGHT))
                direction = Vector2.Right;
            else direction = Vector2.Zero; 
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