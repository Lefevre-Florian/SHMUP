using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Structure {

	public class GameManager : Node
	{

		private static GameManager instance;

		[Export] private NodePath cameraPath = default;

		public Camera2D camera = null;

		private GameManager(): base() { }

		public override void _Ready()
		{
			if(instance != null)
            {
				QueueFree();
				return;
            }
			instance = this;

			camera = GetNode<Camera2D>(cameraPath);
		}

		public static GameManager GetInstance()
        {
			if (instance == null) instance = new GameManager();
			return instance;
        }

        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null) instance = null;
            base.Dispose(pDisposing);
        }

    }

}