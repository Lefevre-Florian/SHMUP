using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class TimerScreen : Screen
	{

		[Export] private float duration;
		[Export] private NodePath btnPath = default;
		[Export] private PackedScene loadScene = default;

		[Export] private NodePath rendererPath = default;
		[Export] private Tween.TransitionType transition = default;
		[Export] private Tween.EaseType ease = default;

		private const string PROPERTY_GLOBALPOSITION = "global_position:x";

		private Button btn = null;

		public override void _Ready()
		{
			base._Ready();

			btn = GetNode<Button>(btnPath);
			btn.Connect(EventButton.PRESSED, this, nameof(Timeout));

			GetTree().CreateTimer(duration).Connect(EventTimer.TIMEOUT, this, nameof(Timeout));
			SceneTreeTween lTween = GetTree().CreateTween();
			lTween.TweenProperty(GetNode<Node2D>(rendererPath), PROPERTY_GLOBALPOSITION, GetViewportRect().Size.x, duration)
				  .SetTrans(transition)
				  .SetEase(ease);
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