using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Environment {

	public class CarLight : Polygon2D
	{

		[Export] private bool isLoop = false;
		[Export (PropertyHint.Range, "0f,1f, 0.1f")] private float ligheningFactor;
		[Export] private float switchingDuration;
		[Export] private Tween.TransitionType transitionType = default;
		[Export] private Tween.EaseType easeType = default;

		private const string PROPERTY_COLOR = "color";

		private Color initialColor;

		public override void _Ready()
		{
			initialColor = Color;
		}

		public void SwitchLight(bool pSwicthState)
        {
			SceneTreeTween lTween = GetTree().CreateTween();
            if (pSwicthState)
            {
				lTween.TweenProperty(this, PROPERTY_COLOR, Color.Lightened(ligheningFactor), switchingDuration)
					.SetTrans(transitionType)
					.SetEase(easeType);
            }
            else
            {
				lTween.TweenProperty(this, PROPERTY_COLOR, initialColor, switchingDuration)
					  .SetTrans(transitionType)
					  .SetEase(easeType);
			}

			if(isLoop)
				lTween.TweenCallback(this, nameof(SwitchLight), new Godot.Collections.Array(!pSwicthState));

		}

	}

}