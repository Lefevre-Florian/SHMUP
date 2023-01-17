using Com.IsartDigital.SHMUP.Structure;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class GameoverPopup : GamePopup
	{

		[Export] private NodePath scoreLabelPath = default;

		Hud hud = null;

		public override void _Ready()
		{
			base._Ready();
			hud = Hud.GetInstance();

			Label lLabel = GetNode<Label>(scoreLabelPath);
			lLabel.Text = hud.GetScoreValue().ToString();
		}

	}

}