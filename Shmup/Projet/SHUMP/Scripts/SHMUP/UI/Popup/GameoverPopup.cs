using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.UI {

	public class GameoverPopup : GamePopup
	{

		[Export] private NodePath scoreLabelPath = default;

		private Hud hud = null;

		private Label scoreLabel = null;

		public override void _Ready()
		{
			base._Ready();
			hud = Hud.GetInstance();

			scoreLabel = GetNode<Label>(scoreLabelPath);
			scoreLabel.Text = hud.GetScoreValue().ToString();
		}

    }

}