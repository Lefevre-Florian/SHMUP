using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class TimerScreen : Screen
	{

		[Export] private float duration;
		[Export] private NodePath btnPath = default;
		[Export] private PackedScene loadScene = default;

		private Button btn = null;

		public override void _Ready()
		{
			base._Ready();

			btn = GetNode<Button>(btnPath);
			btn.Connect(EventButton.PRESSED, this, nameof(Timeout));

			GetTree().CreateTimer(duration).Connect(EventTimer.TIMEOUT, this, nameof(Timeout));
		}

		private void Timeout()
		{
			GetTree().ChangeSceneTo(loadScene);
		}

        protected override void Destructor()
        {
			btn.Disconnect(EventButton.PRESSED, this, nameof(Timeout));
            base.Destructor();
        }


    }

}