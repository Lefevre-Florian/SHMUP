using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.UI {

	public class GameoverPopup : GamePopup
	{

		[Export] private NodePath scoreLabelPath = default;

		[Export] private NodePath scoreProgressPath = default;
		[Export] private float progressDuration;
		[Export] private Tween.TransitionType progressTransition = default;
		[Export] private Tween.EaseType progressEase = default;

		[Export] private float starAnimationDuration;
		[Export] private Tween.TransitionType starAnimationTransition = default;
		[Export] private Tween.EaseType starEaseType = default;

		private const string PROPERTY_MODULATE = "modulate:a";

		private List<float> scoreValues = new List<float>()
		{
			3000f,
			5000f,
			7000f
		};

		private Hud hud = null;

		private Label scoreLabel = null;
		private float value;

		private int scoreIndex = 0;

		private ProgressBar scoreProgress = null;
		private List<Polygon2D> stars = new List<Polygon2D>();

		public override void _Ready()
		{
			base._Ready();
			hud = Hud.GetInstance();

			scoreLabel = GetNode<Label>(scoreLabelPath);
			value = hud.GetScoreValue();

			scoreProgress = GetNode<ProgressBar>(scoreProgressPath);
			foreach (Polygon2D lStar in scoreProgress.GetChildren())
				stars.Add(lStar);

			SceneTreeTween lTween = GetTree().CreateTween();
			lTween.SetPauseMode(SceneTreeTween.TweenPauseMode.Process);
			lTween.TweenMethod(this, nameof(TweenXPProgress), 0f, value, progressDuration)
				  .SetTrans(progressTransition)
				  .SetEase(progressEase);
			lTween.TweenCallback(this, nameof(WriteScore));
			lTween.Play();
		}

		private void TweenXPProgress(float pValue)
        {
			scoreProgress.Value = pValue;
			GD.Print(pValue);
			if (scoreIndex < scoreValues.Count && pValue >= scoreValues[scoreIndex])
            {
				SceneTreeTween lTween = GetTree().CreateTween();
				lTween.SetPauseMode(SceneTreeTween.TweenPauseMode.Process);
				lTween.TweenProperty(stars[scoreIndex].GetChild(0), PROPERTY_MODULATE, 0f, starAnimationDuration)
					  .SetTrans(starAnimationTransition)
					  .SetEase(starEaseType);
				lTween.Play();
				scoreIndex++;
            }
        }

		private void WriteScore()
        {
			scoreLabel.Text = value.ToString();
		}

    }

}