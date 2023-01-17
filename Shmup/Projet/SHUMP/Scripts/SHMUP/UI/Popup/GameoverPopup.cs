using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.UI {

	public class GameoverPopup : GamePopup
	{

		[Export] private NodePath scoreLabelPath = default;
		[Export] private List<int> scorePhase = new List<int>();
		[Export] private NodePath xpProgressPath = default;
		[Export] private float duration;
		[Export] private Tween.TransitionType transitionType = default;
		[Export] private Tween.EaseType easeType = default;

		private const string PROPERTY_VALUE = "value";

		private Hud hud = null;
		private ProgressBar xpProgress = null;

		private Label scoreLabel = null;
		private int scoreIndex = 0;

		public override void _Ready()
		{
			base._Ready();
			hud = Hud.GetInstance();

			scoreLabel = GetNode<Label>(scoreLabelPath);

			xpProgress = GetNode<ProgressBar>(xpProgressPath);

			SceneTreeTween lTween = GetTree().CreateTween();
			lTween.TweenMethod(this, nameof(WriteScore), 0, hud.GetScoreValue(), duration)
				  .SetTrans(transitionType)
				  .SetEase(easeType);
			lTween.Play();
		}

		private void WriteScore(int pValue)
        {
			GD.Print(pValue);
			scoreLabel.Text = pValue.ToString();
        }

    }

}