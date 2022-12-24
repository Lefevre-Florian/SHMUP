using Godot;
using System;
using Com.IsartDigital.SHMUP.UI;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.GameEntities {

	public class FlyingScore : Label
	{

		[Export] private float animationSpeed = 1f;
		[Export] private Tween.TransitionType transitionType = default;
		[Export] private Tween.EaseType easeType = default;

		private const string PROPERTY_GLOBALPOSITION = "rect_position";

		private Hud hud = null;
		private uint score = 0;

		public void SetScore(uint pScore)
        {
			hud = Hud.GetInstance();
			score = pScore;

			Text = score.ToString();

			SceneTreeTween lTween = GetTree().CreateTween();
			lTween
				.TweenProperty(this, PROPERTY_GLOBALPOSITION, hud.GetScorePosition(), animationSpeed)
				.SetTrans(transitionType)
				.SetEase(easeType);
			lTween.TweenCallback(this, nameof(Destroy));
			lTween.Play();
        }

		private void Destroy()
        {
			hud.UpdateScoreHUD(score);
			QueueFree();
        }

	}

}